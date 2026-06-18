import api from "./api";
import type { SeatStatus } from "../types/index";

export const getSeatsByBusId = async (busId: number): Promise<SeatStatus[]> => {
  const response = await api.get(`/seat/${busId}/seats`);
  return response.data;
};

export const lockSeat = async (seatId: number): Promise<string> => {
  const response = await api.post(`/seat/${seatId}/lock`);
  return response.data;
};

export const releaseLock = async (seatId: number): Promise<string> => {
  const response = await api.delete(`/seat/${seatId}/lock`);
  return response.data;
};
