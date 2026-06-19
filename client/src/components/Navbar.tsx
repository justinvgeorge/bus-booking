import { Link, useNavigate } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'

export default function Navbar() {
  const { user, logout, isAuthenticated } = useAuth()
  const navigate = useNavigate()

  const handleLogout = () => {
    logout()
    navigate('/login')
  }

  return (
    <nav className="bg-blue-600 text-white px-6 py-4 flex justify-between items-center">
      <Link to="/search" className="text-x1 font-bold">
        BusBook
      </Link>

      <div className="flex items-center gap-4">
        {isAuthenticated ? (
          <>
            <span className="text-sm">Hello, {user?.fullName}</span>
            <Link to="/my-bookings" className="hover:underline">
              My Bookings
            </Link>
            <button
              onClick={handleLogout}
              className="bg-white text-blue-600 px-3 py-1 rounded font-medium hover:bg-gray-100"
            >
              Logout
            </button>
          </>
        ) : (
          <>
            <Link to="/login" className="hover:underline">
              Login
            </Link>
            <Link to="/register" className="bg-white text-blue-600 px-3 py-1 rounded font-medium hover:bg-gray-100">
              Register
            </Link>
          </>
        )}
      </div>
    </nav>
  )
}
