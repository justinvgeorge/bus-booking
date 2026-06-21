import api from './api'
import type { Schedule, PaginatedResult } from '../types/index'

export const searchSchedule = async (
  origin: string,
  destination: string,
  date: string,
  page: number = 1,
  pageSize: number = 5
): Promise<PaginatedResult<Schedule>> => {
  const response = await api.get('/schedule/search', {
    params: { origin, destination, date, page, pageSize },
  })
  return response.data
}

export const getAllSchedules = async (): Promise<Schedule[]> => {
  const response = await api.get('/schedule')
  return response.data
}

export const getScheduleById = async (id: number): Promise<Schedule> => {
  const response = await api.get(`/schedule/${id}`)
  return response.data
}
