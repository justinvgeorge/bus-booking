export interface AuthResponse {
  token: string
  fullName: string
  email: string
  role: string
  id: number
}

export interface PaginatedResult<T> {
  items: T[]
  totalCount: number
  page: number
  pageSize: number
  totalPages: number
}

export interface Schedule {
  id: number
  departure: string
  arrival: string
  price: number
  status: string
  busId: number
  busNumber: string
  busType: string
  origin: string
  destination: string
}

export interface Bus {
  id: number
  busNumber: string
  busType: string
  totalSeats: number
  isActive: boolean
}

export interface BusRoute {
  id: number
  origin: string
  destination: string
  distance: number
  duration: string
}

export interface SeatStatus {
  id: number
  seatNumber: string
  seatType: string
  isAvailable: boolean
  isLocked: boolean
  lockedByUserId: string | null
}

export interface Booking {
  id: number
  bookingDate: string
  totalPrice: number
  status: string
  userFullName: string
  departureTime: string
  arrivalTime: string
  origin: string
  destination: string
  busNumber: string
  seatNumber: string
  seatType: string
}

export interface RegisterRequest {
  fullName: string
  email: string
  password: string
  phone?: string
}

export interface LoginRequest {
  email: string
  password: string
}
