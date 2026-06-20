import { useEffect, useState } from 'react'
import { useParams, useNavigate } from 'react-router-dom'
import type { SeatStatus } from '../types'
import { getSeatsByBusId, lockSeat, releaseLock } from '../services/seatService'
import { useAuth } from '../context/AuthContext'

export default function SeatMapPage() {
  const [seats, setSeats] = useState<SeatStatus[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [selectedSeat, setSelectedSeat] = useState<SeatStatus | null>(null)
  const [countdown, setCountDown] = useState<number | null>(null)
  const [locking, setLocking] = useState(false)
  const [lockMessage, setLockMessage] = useState('')
  const [refreshing, setRefreshing] = useState(false)

  const { scheduleId, busId } = useParams()
  const navigate = useNavigate()
  const { user } = useAuth()

  const refreshSeats = async () => {
    setError('')
    setLockMessage('')
    setRefreshing(true)
    try {
      const result = await getSeatsByBusId(Number(busId))
      setSeats(result)
    } finally {
      setRefreshing(false)
    }
  }

  useEffect(() => {
    const loadSeats = async () => {
      try {
        const result = await getSeatsByBusId(Number(busId))
        setSeats(result)
      } catch {
        setError('Failed to load seats.')
      } finally {
        setLoading(false)
      }
    }
    loadSeats()
    const interval = setInterval(loadSeats, 30000)
    return () => clearInterval(interval)
  }, [busId])

  useEffect(() => {
    if (countdown === null) return
    if (countdown === 0) {
      const reset = setTimeout(() => {
        setSelectedSeat(null)
        setCountDown(null)
        getSeatsByBusId(Number(busId)).then(setSeats)
      }, 0)
      return () => clearTimeout(reset)
    }
    const timer = setTimeout(() => setCountDown(c => (c ?? 0) - 1), 1000)
    return () => clearTimeout(timer)
  }, [countdown, busId])

  const handleSeatClick = async (seat: SeatStatus) => {
    setError('')
    setLockMessage('')
    if (seat.isLocked && seat.lockedByUserId !== String(user?.id)) {
      await refreshSeats()
      setLockMessage('')
      setTimeout(
        () => setLockMessage('This seat is locked by another user'),
        50
      )
      return
    }

    if (seat.isLocked && seat.lockedByUserId === String(user?.id)) {
      try {
        await releaseLock(seat.id)
        setSelectedSeat(null)
        setCountDown(null)
        setLockMessage('')
        await refreshSeats()
      } catch {
        setError('Failed to release lock.')
      }
      return
    }

    if (!seat.isAvailable) return

    setLocking(true)
    try {
      await lockSeat(seat.id)
      setSelectedSeat(seat)
      setCountDown(60)
      const result = await getSeatsByBusId(Number(busId))
      setSeats(result)
    } catch (err: unknown) {
      if (err && typeof err === 'object' && 'response' in err) {
        const axiosError = err as { response?: { status?: number } }
        if (axiosError.response?.status === 409) {
          setLockMessage('')
          setTimeout(
            () => setLockMessage('This seat is locked by another user'),
            50
          )
          await refreshSeats()
        } else {
          setError('Failed to lock seat. Please try again.')
        }
      } else {
        setError('Failed to lock seat. Please try again.')
      }
    } finally {
      setLocking(false)
    }
  }

  const getSeatColor = (seat: SeatStatus) => {
    if (!seat.isAvailable) return 'bg-red-400 cursor-not-allowed text-white'
    if (seat.isLocked && seat.lockedByUserId === String(user?.id))
      return 'bg-blue-500 cursor-pointer text-white'
    if (seat.isLocked) return 'bg-yellow-400 cursor-not-allowed text-white'
    return 'bg-green-400 cursor-pointer text-white hover:bg-green-500'
  }

  if (loading) return <div className="text-center p-10">Loading seats...</div>

  return (
    <div className="min-h-screen bg-gray-50 p-6">
      <div className="max-w-2xl mx-auto">
        <h1 className="text-2xl font-bold text-blue-600 mb-2 text-center">
          Select Your Seat
        </h1>

        {error && (
          <div className="bg-red-100 text-red-600 px-4 py-2 rounded mb-4 text-sm">
            {error}
          </div>
        )}

        {/* Legend */}
        <div className="flex gap-4 justify-center mb-6 text-sm">
          <span className="flex items-center gap-1">
            <span className="w-4 h-4 bg-green-400 rounded inline-block" />{' '}
            Available
          </span>
          <span className="flex items-center gap-1">
            <span className="w-4 h-4 bg-yellow-400 rounded inline-block" />{' '}
            Locked
          </span>
          <span className="flex items-center gap-1">
            <span className="w-4 h-4 bg-blue-500 rounded inline-block" /> Your
            selection
          </span>
          <span className="flex items-center gap-1">
            <span className="w-4 h-4 bg-red-400 rounded inline-block" /> Booked
          </span>
        </div>
        {/* Refresh button */}
        <div className="flex justify-end mb-2">
          <button
            onClick={refreshSeats}
            disabled={refreshing}
            className="text-sm text-blue-600 hover:underline disabled:opacity-50"
          >
            {refreshing ? 'Refreshing...' : '↻ Refresh seats'}
          </button>
        </div>
        {/* Lock message */}
        {lockMessage && (
          <div className="bg-yellow-100 text-yellow-700 px-4 py-2 rounded mb-4 text-sm text-center">
            {lockMessage}
          </div>
        )}

        {/* Seat grid */}
        <div className="bg-white p-6 rounded-lg shadow-md">
          <div className="grid grid-cols-4 gap-3">
            {seats.map(seat => (
              <button
                key={seat.id}
                onClick={() => handleSeatClick(seat)}
                disabled={
                  !seat.isAvailable ||
                  (seat.isLocked && seat.lockedByUserId !== String(user?.id)) ||
                  locking
                }
                className={`p-3 rounded font-medium text-sm ${getSeatColor(seat)}`}
              >
                {seat.seatNumber}
              </button>
            ))}
          </div>
        </div>

        {/* Countdown + confirm */}
        {selectedSeat && countdown !== null && (
          <div className="mt-6 bg-blue-50 border border-blue-200 rounded-lg p-4 text-center">
            <p className="text-blue-700 font-medium">
              Seat {selectedSeat.seatNumber} locked!
            </p>
            <p className="text-blue-500 text-sm mt-1">
              Time remaining: {countdown} seconds
            </p>
            <button
              onClick={() =>
                navigate(`/confirm/${scheduleId}/${selectedSeat.id}`)
              }
              className="mt-3 bg-blue-600 text-white px-6 py-2 rounded hover:bg-blue-700"
            >
              Confirm Booking
            </button>
          </div>
        )}
      </div>
    </div>
  )
}
