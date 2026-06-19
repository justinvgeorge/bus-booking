export interface AuthResponse {
  token: string
  fullName: string
  email: string
  role: string
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
