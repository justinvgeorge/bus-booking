using BusBooking.API.Models;
using BusBooking.API.Repositories.Interfaces;
using BusBooking.API.Services.Interfaces;
using StackExchange.Redis;

namespace BusBooking.API.Services
{
    public class SeatService : ISeatService
    {
        private readonly ISeatRepository _seatRepository;
        private readonly IConnectionMultiplexer _redis;
        private readonly IConfiguration _configuration;

        public SeatService (ISeatRepository seatRepository, IConnectionMultiplexer redis, IConfiguration configuration)
        {
            _seatRepository = seatRepository;
            _redis = redis;
            _configuration = configuration;
        }

        private const string SeatLockPrefix = "seat:lock:";

        public async Task<bool> LockSeatAsync(int seatId, int userId)
        {

            var lockDuration = int.Parse(_configuration["SeatLockSettings:LockDurationMinutes"]!);
            var db = _redis.GetDatabase();
            var key = $"{SeatLockPrefix}{seatId}";

            return await db.StringSetAsync(key, userId.ToString(), TimeSpan.FromMinutes(lockDuration), When.NotExists); 
        }
        public async Task ReleaseLockAsync(int seatId)
        {
            var db = _redis.GetDatabase();
            await db.KeyDeleteAsync($"{SeatLockPrefix}{seatId}");

        }
        public async Task<string?> GetSeatLockOwnerAsync(int seatId)
        {
            var db = _redis.GetDatabase();
            var value = await db.StringGetAsync($"{SeatLockPrefix}{seatId}");
            return value.HasValue ? value.ToString() : null;
        }
        public async Task<bool> IsSeatLockedAsync(int seatId)
        {
            var db = _redis.GetDatabase();
            return await db.KeyExistsAsync($"{SeatLockPrefix}{seatId}");
        }
        public async Task<IEnumerable<Seat>> GetSeatsByBusIdAsync(int busId)
        {
            return await _seatRepository.GetByBusIdAsync(busId);
        }


    }
}
