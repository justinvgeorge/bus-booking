import api from "./api";
import type { Schedule } from "../types/index";

export const searchSchedule = async (
  origin: string,
  destination: string,
  date: string,
): Promise<Schedule[]> => {
  const response = await api.get("/schedule/search", {
    params: { origin, destination, date },
  });
  return response.data;
};

export const getAllSchedules = async (): Promise<Schedule[]> => {
  const response = await api.get("/schedule");
  return response.data;
};

export const getScheduleById = async (id: number): Promise<Schedule> => {
  const response = await api.get(`/schedule/${id}`);
  return response.data;
};
