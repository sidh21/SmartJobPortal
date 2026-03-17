import axios from 'axios';
import { ApiResponse, AuthResponse } from '../types';

const BASE_URL = process.env.REACT_APP_AUTH_API || 'https://smartjobportal-auth-2.onrender.com/api/Auth';

export const registerApi = async (
  fullName: string,
  email: string,
  password: string,
  role: string
): Promise<ApiResponse<AuthResponse>> => {
  const response = await axios.post(`${BASE_URL}/register`, {
    fullName,
    email,
    password,
    role
  });
  return response.data;
};

export const loginApi = async (
  email: string,
  password: string
): Promise<ApiResponse<AuthResponse>> => {
  const response = await axios.post(`${BASE_URL}/login`, {
    email,
    password
  });
  return response.data;
};