import api from "./api";
import type { AuthResponse, RegisterRequest, LoginRequest } from "../types";

export const register = async (
  data: RegisterRequest,
): Promise<AuthResponse> => {
  const response = await api.post("/auth/register", data);
  return response.data;
};

export const login = async (data: LoginRequest): Promise<AuthResponse> => {
  const response = await api.post("/auth/login", data);
  return response.data;
};
