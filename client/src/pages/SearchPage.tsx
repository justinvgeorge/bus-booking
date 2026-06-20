import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { searchSchedule as scheduleService } from '../services/scheduleService'
import type { Schedule } from '../types'

export default function SearchPage() {
  const [origin, setOrigin] = useState('')
  const [destination, setDestination] = useState('')
  const [date, setDate] = useState('')
  const [schedules, setSchedules] = useState<Schedule[]>([])
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState('')
  const [searched, setSearched] = useState(false)

  const navigate = useNavigate()

  const handleSubmit = async (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault()
    setError('')
    setLoading(true)
    setSearched(true)
    try {
      const result = await scheduleService(origin, destination, date)
      setSchedules(result)
    } catch (error) {
      if (error && typeof error == 'object' && 'response' in error) {
        const axiosError = error as { response?: { data?: string } }
        setError(
          axiosError.response?.data || 'Search failed. Please try again.'
        )
      } else {
        setError('Search failed. Please try again.')
      }
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="min-h-screen bg-gray-50 p-6">
      <div className="max-w-3xl mx-auto">
        <h1 className="text-3xl font-bold text-blue-600 mb-6 text-center">
          Find your bus
        </h1>
        <form
          onSubmit={handleSubmit}
          className="bg-white p-6 rounded-lg shadow-md flex flex-col gap-4"
        >
          <div className="flex gap-4">
            <div className="flex-1">
              <label className="block text-sm font-medium text-gray-700 mb-1">
                From
              </label>
              <input
                type="text"
                value={origin}
                onChange={e => {
                  setOrigin(e.target.value)
                }}
                required
                placeholder="Bangalore"
                className="w-full border border-gray-300 rounded px-3 py-2 focus:outline-none focus:border-blue-500"
              />
            </div>
            <div className="flex-1">
              <label className="block text-sm font-medium text-gray-700 mb-1">
                To
              </label>
              <input
                type="text"
                value={destination}
                onChange={e => {
                  setDestination(e.target.value)
                }}
                required
                placeholder="Chennai"
                className="w-full border border-gray-300 rounded px-3 py-2 focus:outline-none focus:border-blue-500"
              />
            </div>
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Date
            </label>
            <input
              type="date"
              value={date}
              onChange={e => {
                setDate(e.target.value)
              }}
              required
              className="w-full border border-gray-300 rounded px-3 py-2 focus:outline-none focus:border-blue-500"
            />
          </div>
          <button
            type="submit"
            disabled={loading}
            className="bg-blue-600 text-white py-2 rounded font-medium hover:bg-blue-700 disabled:opacity-50"
          >
            {loading ? 'Searching...' : 'Search Buses'}
          </button>
        </form>
        {error && (
          <div className="bg-red-100 text-red-600 px-4 py-2 rounded mt-4 text-sm">
            {error}
          </div>
        )}
        {searched && !loading && schedules.length === 0 && !error && (
          <p className="text-center text-gray-500 mt-8">
            No buses found for this route and date.
          </p>
        )}
        <div className="flex flex-col gap-4 mt-6">
          {schedules.map(schedule => {
            return (
              <div
                key={schedule.id}
                className="bg-white p-6 rounded-lg shadow-md flex justify-between items-center"
              >
                <div>
                  <h2 className="text-lg font-bold text-gray-800">
                    {schedule.origin} → {schedule.destination}
                  </h2>
                  <p className="text-sm text-gray-500 mt-1">
                    {schedule.busNumber} · {schedule.busType}
                  </p>
                  <p className="text-sm text-gray-600 mt-1">
                    {new Date(schedule.departure).toLocaleTimeString()} →{' '}
                    {new Date(schedule.arrival).toLocaleTimeString()}
                  </p>
                </div>
                <div className="text-right">
                  <p className="text-2xl font-bold text-blue-600">
                    ₹{schedule.price}
                  </p>
                  <button
                    onClick={() =>
                      navigate(`/seats/${schedule.id}/${schedule.busId}`)
                    }
                    className="mt-2 bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700 text-sm"
                  >
                    Select Seats
                  </button>
                </div>
              </div>
            )
          })}
        </div>
      </div>
    </div>
  )
}
