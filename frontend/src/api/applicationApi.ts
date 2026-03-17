import axios from 'axios';
import { ApiResponse, Application, CreateApplicationDto } from '../types';

const BASE_URL = process.env.REACT_APP_APPLICATION_API || 'https://smartjobportal-application.onrender.com/api/Application';

const getHeaders = () => ({
  headers: {
    Authorization: `Bearer ${localStorage.getItem('token')}`
  }
});

export const createApplicationApi = async (
  dto: CreateApplicationDto
): Promise<ApiResponse<number>> => {
  const response = await axios.post(BASE_URL, dto, getHeaders());
  return response.data;
};

export const getApplicationsByJobIdApi = async (
  jobId: number
): Promise<ApiResponse<Application[]>> => {
  const response = await axios.get(
    `${BASE_URL}/job/${jobId}`,
    getHeaders()
  );
  return response.data;
};

export const updateApplicationStatusApi = async (
  applicationId: number,
  status: string
): Promise<ApiResponse<boolean>> => {
  const response = await axios.put(
    `${BASE_URL}/${applicationId}/status`,
    { status },
    getHeaders()
  );
  return response.data;
};