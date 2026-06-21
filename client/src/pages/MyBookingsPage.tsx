import { useState, useEffect } from 'react'
import { getMyBookings, cancelBooking } from '../services/bookingService'
import type { Booking } from '../types/index'

export default function MyBookingsPage() {
  const [bookings, setBooking] = useState<Booking[]>([])
  const [error, setError] = useState('')
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    const fetchBooking = async () => {
      try {
        const result = await getMyBookings()
        setBooking(result)
      } catch {
        setError('Unable to fetch your bookings. Please try again.')
      } finally {
        setLoading(false)
      }
    }
    fetchBooking()
  }, [])

  const handleCancel = async (id: number) => {
    try {
      await cancelBooking(id)
      const result = await getMyBookings()
      setBooking(result)
    } catch {
      setError('Failed to cancel bookings.')
    }
  }

  if (loading) return <div className="text-center p-10">Loading...</div>

  return (
    <div className="min-h-screen bg-gray-50 p-6">
      <div className="max-w-3xl mx-auto">
        <h1 className="text-2xl font-bold text-blue-600 mb-6 text-center">
          My Bookings
        </h1>

        {error && (
          <div className="bg-red-100 text-red-600 px-4 py-2 rounded mb-4 text-sm">
            {error}
          </div>
        )}

        {bookings.length === 0 && !error && (
          <p className="text-center text-gray-500 mt-8">
            You have no bookings yet.
          </p>
        )}

        <div className="flex flex-col gap-4">
          {bookings.map(booking => (
            <div key={booking.id} className="bg-white p-6 rounded-lg shadow-md">
              <div className="flex justify-between items-start">
                <div className="flex flex-col gap-2">
                  <h2 className="text-lg font-bold text-gray-800">
                    {booking.origin} → {booking.destination}
                  </h2>
                  <p className="text-sm text-gray-500">{booking.busNumber}</p>
                  <p className="text-sm text-gray-600">
                    🕐 {new Date(booking.departureTime).toLocaleString()}
                  </p>
                  <p className="text-sm text-gray-600">
                    Seat: {booking.seatNumber}
                  </p>
                  <p className="text-sm text-gray-600">
                    Booked on:{' '}
                    {new Date(booking.bookingDate).toLocaleDateString()}
                  </p>
                </div>

                <div className="text-right flex flex-col gap-2 items-end">
                  <p className="text-2xl font-bold text-blue-600">
                    ₹{booking.totalPrice}
                  </p>
                  <span
                    className={`text-xs px-2 py-1 rounded font-medium ${
                      booking.status === 'Confirmed'
                        ? 'bg-green-100 text-green-700'
                        : 'bg-red-100 text-red-700'
                    }`}
                  >
                    {booking.status}
                  </span>
                  {booking.status === 'Confirmed' && (
                    <button
                      onClick={() => handleCancel(booking.id)}
                      className="text-sm text-red-600 hover:underline mt-1"
                    >
                      Cancel Booking
                    </button>
                  )}
                </div>
              </div>
            </div>
          ))}
        </div>
      </div>
    </div>
  )
}
