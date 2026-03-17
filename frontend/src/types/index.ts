export interface User {
  userId: number;
  fullName: string;
  email: string;
  role: 'Candidate' | 'Employer';
  token: string;
  expiresAt: string;
}

export interface Job {
  jobId: number;
  title: string;
  company: string;
  description: string;
  location: string;
  salary: number;
  jobType: 'FullTime' | 'PartTime' | 'Remote';
  isActive: boolean;
  createdAt: string;
}

export interface Application {
  applicationId: number;
  jobId: number;
  candidateName: string;
  candidateEmail: string;
  resumeText: string;
  coverLetter?: string;
  status: 'Pending' | 'Shortlisted' | 'Rejected';
  appliedAt: string;
}

export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
  errors: string[];
}

export interface AuthResponse {
  userId: number;
  fullName: string;
  email: string;
  role: "Candidate" | "Employer";
  token: string;
  expiresAt: string;
}

export interface ScreenResumeResult {
  matchScore: number;
  strengths: string[];
  gaps: string[];
  recommendation: string;
}

export interface CreateJobDto {
  title: string;
  company: string;
  description: string;
  location: string;
  salary: number;
  jobType: string;
}

export interface CreateApplicationDto {
  jobId: number;
  candidateName: string;
  candidateEmail: string;
  resumeText: string;
  coverLetter?: string;
}

export interface ScreenResumeDto {
  applicationId: number;
  jobId: number;
  resumeText: string;
  jobDescription: string;
}

export interface GenerateJDDto {
  jobTitle: string;
  company: string;
  location: string;
  jobType: string;
}