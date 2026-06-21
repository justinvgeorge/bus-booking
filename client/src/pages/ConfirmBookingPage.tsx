import { useState, useEffect } from 'react'
import { useParams, useNavigate } from 'react-router-dom'
import { getScheduleById } from '../services/scheduleService'
import { createBooking } from '../services/bookingService'
import { releaseLock } from '../services/seatService'
import type { Schedule } from '../types'

export default function ConfirmBookingPage() {
  const { scheduleId, seatId } = useParams()
  const navigate = useNavigate()

  const [schedule, setSchedule] = useState<Schedule | null>(null)
  const [confirming, setConfirming] = useState(false)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')

  useEffect(() => {
    const loadSchedule = async () => {
      try {
        const result = await getScheduleById(Number(scheduleId))
        setSchedule(result)
      } catch {
        setError('Unable to load schedule.')
      } finally {
        setLoading(false)
      }
    }
    loadSchedule()
  }, [scheduleId])

  const handleConfirm = async () => {
    setConfirming(true)
    try {
      await createBooking(Number(scheduleId), Number(seatId))
      navigate('/my-bookings')
    } catch (err: unknown) {
      if (err && typeof err === 'object' && 'response' in err) {
        const axiosError = err as { response?: { data: string } }
        setError(
          axiosError.response?.data || 'Booking failed. Please try again.'
        )
      } else {
        setError('Booking failed. Please try again.')
      }
    } finally {
      setConfirming(false)
    }
  }

  const handleCancel = async () => {
    try {
      await releaseLock(Number(seatId))
    } finally {
      navigate(-1)
    }
  }

  if (loading) return <div className="text-center p-10">Loading...</div>

  return (
    <div className="min-h-screen bg-gray-50 p-6">
      <div className="max-w-lg mx-auto">
        <h1 className="text-2xl font-bold text-blue-600 mb-6 text-center">
          Confirm Your Booking
        </h1>

        {error && (
          <div className="bg-red-100 text-red-600 px-4 py-2 rounded mb-4 text-sm">
            {error}
          </div>
        )}

        {schedule && (
          <div className="bg-white rounded-lg shadow-md p-6 flex flex-col gap-4">
            {/* Route */}
            <div>
              <p className="text-sm text-gray-500">Route</p>
              <p className="text-lg font-bold text-gray-800">
                {schedule.origin} → {schedule.destination}
              </p>
            </div>

            {/* Bus details */}
            <div>
              <p className="text-sm text-gray-500">Bus</p>
              <p className="text-gray-800">
                {schedule.busNumber} · {schedule.busType}
              </p>
            </div>

            {/* Time */}
            <div>
              <p className="text-sm text-gray-500">Departure</p>
              <p className="text-gray-800">
                {new Date(schedule.departure).toLocaleString()}
              </p>
            </div>

            <div>
              <p className="text-sm text-gray-500">Arrival</p>
              <p className="text-gray-800">
                {new Date(schedule.arrival).toLocaleString()}
              </p>
            </div>

            {/* Seat */}
            <div>
              <p className="text-sm text-gray-500">Seat</p>
              <p className="text-gray-800">#{seatId}</p>
            </div>

            {/* Price */}
            <div className="border-t pt-4">
              <p className="text-sm text-gray-500">Total Price</p>
              <p className="text-2xl font-bold text-blue-600">
                ₹{schedule.price}
              </p>
            </div>

            {/* Buttons */}
            <div className="flex gap-3 mt-2">
              <button
                onClick={handleCancel}
                className="flex-1 border border-gray-300 text-gray-700 py-2 rounded hover:bg-gray-50"
              >
                Cancel
              </button>
              <button
                onClick={handleConfirm}
                disabled={confirming}
                className="flex-1 bg-blue-600 text-white py-2 rounded hover:bg-blue-700 disabled:opacity-50"
              >
                {confirming ? 'Confirming...' : 'Confirm Booking'}
              </button>
            </div>
          </div>
        )}
      </div>
    </div>
  )
}
