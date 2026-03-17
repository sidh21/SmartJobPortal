import axios from 'axios';
import { ApiResponse, GenerateJDDto, ScreenResumeDto, ScreenResumeResult } from '../types';

const BASE_URL = 'https://localhost:7003/api/AI';

const getHeaders = () => ({
  headers: {
    Authorization: `Bearer ${localStorage.getItem('token')}`
  }
});

export const screenResumeApi = async (
  dto: ScreenResumeDto
): Promise<ApiResponse<ScreenResumeResult>> => {
  const response = await axios.post(
    `${BASE_URL}/screen-resume`,
    dto,
    getHeaders()
  );
  return response.data;
};

export const generateJDApi = async (
  dto: GenerateJDDto
): Promise<ApiResponse<string>> => {
  const response = await axios.post(
    `${BASE_URL}/generate-jd`,
    dto,
    getHeaders()
  );
  return response.data;
};

export const rankCandidatesApi = async (
  jobDescription: string,
  candidates: { applicationId: number; candidateName: string; resumeText: string }[]
): Promise<ApiResponse<string>> => {
  const response = await axios.post(
    `${BASE_URL}/rank-candidates`,
    { jobDescription, candidates },
    getHeaders()
  );
  return response.data;
};