import api from "./api";
import type { Booking } from "../types/index";

export const createBooking = async (
  scheduleId: number,
  seatId: number,
): Promise<Booking> => {
  const response = await api.post("/booking", { scheduleId, seatId });
  return response.data;
};

export const getMyBookings = async (): Promise<Booking[]> => {
  const response = await api.get(`/booking`);
  return response.data;
};

export const getBookingById = async (id: number): Promise<Booking> => {
  const response = await api.get(`/booking/${id}`);
  return response.data;
};

export const cancelBooking = async (id: number): Promise<void> => {
  await api.delete(`/booking/${id}/cancel`);
};
