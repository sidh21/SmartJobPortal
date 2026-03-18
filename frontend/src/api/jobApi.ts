import axios from 'axios';
import { ApiResponse, CreateJobDto, Job } from '../types';

const BASE_URL = process.env.REACT_APP_JOB_API || 'https://smartjobportal-job.onrender.com/api/Job';

// ✅ Configure axios instance with credentials
const axiosInstance = axios.create({
  baseURL: BASE_URL,
  withCredentials: true, // Important for CORS with credentials
  headers: {
    'Content-Type': 'application/json'
  }
});

// ✅ Add request interceptor to attach token
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

export const getAllJobsApi = async (): Promise<ApiResponse<Job[]>> => {
  const response = await axiosInstance.get('');
  return response.data;
};

export const getJobByIdApi = async (
  jobId: number
): Promise<ApiResponse<Job>> => {
  const response = await axiosInstance.get(`/${jobId}`);
  return response.data;
};

export const createJobApi = async (
  dto: CreateJobDto
): Promise<ApiResponse<number>> => {
  const response = await axiosInstance.post('', dto);
  return response.data;
};

export const updateJobApi = async (
  jobId: number,
  dto: CreateJobDto
): Promise<ApiResponse<boolean>> => {
  const response = await axiosInstance.put(`/${jobId}`, dto);
  return response.data;
};

export const deleteJobApi = async (
  jobId: number
): Promise<ApiResponse<boolean>> => {
  const response = await axiosInstance.delete(`/${jobId}`);
  return response.data;
};