import axios from 'axios';
import { ApiResponse, GenerateJDDto, ScreenResumeDto, ScreenResumeResult } from '../types';

const BASE_URL = process.env.REACT_APP_AI_API || 'https://smartjobportal-ai.onrender.com/api/AI';

// ✅ Simplified without credentials
const axiosInstance = axios.create({
  baseURL: BASE_URL,
  headers: {
    'Content-Type': 'application/json'
  }
});

// Add token interceptor
axiosInstance.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

export const screenResumeApi = async (
  dto: ScreenResumeDto
): Promise<ApiResponse<ScreenResumeResult>> => {
  const response = await axiosInstance.post('/screen-resume', dto);
  return response.data;
};

export const generateJDApi = async (
  dto: GenerateJDDto
): Promise<ApiResponse<string>> => {
  const response = await axiosInstance.post('/generate-jd', dto);
  return response.data;
};

export const rankCandidatesApi = async (
  jobDescription: string,
  candidates: { applicationId: number; candidateName: string; resumeText: string }[]
): Promise<ApiResponse<string>> => {
  const response = await axiosInstance.post('/rank-candidates', {
    jobDescription,
    candidates
  });
  return response.data;
};