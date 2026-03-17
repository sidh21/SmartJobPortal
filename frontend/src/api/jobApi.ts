import axios from 'axios';
import { ApiResponse, CreateJobDto, Job } from '../types';

const BASE_URL = process.env.REACT_APP_JOB_API || 'https://smartjobportal-job.onrender.com/api/Job';

const getHeaders = () => ({
  headers: {
    Authorization: `Bearer ${localStorage.getItem('token')}`
  }
});

export const getAllJobsApi = async (): Promise<ApiResponse<Job[]>> => {
  const response = await axios.get(BASE_URL, getHeaders());
  return response.data;
};

export const getJobByIdApi = async (
  jobId: number
): Promise<ApiResponse<Job>> => {
  const response = await axios.get(`${BASE_URL}/${jobId}`, getHeaders());
  return response.data;
};

export const createJobApi = async (
  dto: CreateJobDto
): Promise<ApiResponse<number>> => {
  const response = await axios.post(BASE_URL, dto, getHeaders());
  return response.data;
};

export const updateJobApi = async (
  jobId: number,
  dto: CreateJobDto
): Promise<ApiResponse<boolean>> => {
  const response = await axios.put(`${BASE_URL}/${jobId}`, dto, getHeaders());
  return response.data;
};

export const deleteJobApi = async (
  jobId: number
): Promise<ApiResponse<boolean>> => {
  const response = await axios.delete(`${BASE_URL}/${jobId}`, getHeaders());
  return response.data;
};