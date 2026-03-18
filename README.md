# 🚀 SmartJobPortal - AI-Powered Job Platform

[![C#](https://img.shields.io/badge/C%23-512BD4?style=for-the-badge&logo=csharp&logoColor=white)](https://dotnet.microsoft.com/)
[![.NET 9.0](https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![React](https://img.shields.io/badge/React-20232A?style=for-the-badge&logo=react&logoColor=61DAFB)](https://reactjs.org/)
[![TypeScript](https://img.shields.io/badge/TypeScript-007ACC?style=for-the-badge&logo=typescript&logoColor=white)](https://www.typescriptlang.org/)
[![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)](https://www.docker.com/)
[![Render](https://img.shields.io/badge/Render-46E3B7?style=for-the-badge&logo=render&logoColor=white)](https://render.com/)
[![Vercel](https://img.shields.io/badge/Vercel-000000?style=for-the-badge&logo=vercel&logoColor=white)](https://vercel.com/)
[![Google Gemini](https://img.shields.io/badge/Google%20Gemini-8E75B2?style=for-the-badge&logo=googlegemini&logoColor=white)](https://deepmind.google/technologies/gemini/)

<p align="center">
  <img src="https://via.placeholder.com/800x400.png?text=SmartJobPortal+Demo+Screenshot" alt="SmartJobPortal Demo" width="800"/>
</p>

## 📋 Table of Contents
- [✨ Overview](#-overview)
- [🏗️ Architecture](#️-architecture)
- [🚀 Live Demo](#-live-demo)
- [✨ Features](#-features)
- [🛠️ Tech Stack](#️-tech-stack)
- [📦 Microservices](#-microservices)
- [🎯 AI Integration](#-ai-integration)
- [🏃 Getting Started](#-getting-started)
- [🌐 Deployment](#-deployment)
- [📸 Screenshots](#-screenshots)
- [📊 Project Statistics](#-project-statistics)
- [🧪 Testing the API](#-testing-the-api)
- [🛣️ Roadmap](#️-roadmap)
- [👥 Contributors](#-contributors)
- [📄 License](#-license)
- [📧 Contact](#-contact)

## ✨ Overview

**SmartJobPortal** is a modern, AI-powered job platform that connects talented candidates with their dream employers. Built with a **microservices architecture** on .NET 9.0 and a sleek React TypeScript frontend, it leverages **Google Gemini AI** to revolutionize the recruitment process.

> "Finding the perfect job match has never been smarter!"


## 🚀 Live Demo

| Component | URL | Status |
|-----------|-----|--------|
| **Frontend Application** | [https://smartjobportal-frontend.vercel.app](https://smartjobportal-frontend.vercel.app) | ✅ Live |
| **API Gateway** | [https://smartjobportal-gateway.onrender.com](https://smartjobportal-gateway.onrender.com) | ✅ Live |
| **Auth Service** | [https://smartjobportal-auth-2.onrender.com/swagger](https://smartjobportal-auth-2.onrender.com/swagger) | ✅ Live |
| **Job Service** | [https://jobservice-5xon.onrender.com/swagger](https://jobservice-5xon.onrender.com/swagger) | ✅ Live |
| **Application Service** | [https://smartjobportal-application.onrender.com/swagger](https://smartjobportal-application.onrender.com/swagger) | ✅ Live |
| **AI Service** | [https://smartjobportal-ai.onrender.com/swagger](https://smartjobportal-ai.onrender.com/swagger) | ✅ Live |

## ✨ Features

### 👨‍💼 For Employers
- ✅ **AI-Powered Job Description Generation** - Create professional JDs in seconds with Google Gemini
- ✅ **Smart Resume Screening** - Let AI analyze and rank candidates automatically
- ✅ **Candidate Matching** - Find the perfect fit with AI-powered matching algorithms
- ✅ **Application Management** - Track and manage all applications in one dashboard
- ✅ **Analytics Dashboard** - Get insights into job postings and applicant trends

### 👨‍🎓 For Candidates
- ✅ **Intelligent Job Search** - Find jobs that match your skills and experience
- ✅ **One-Click Apply** - Submit applications effortlessly with saved profiles
- ✅ **Resume Upload** - Let employers discover you through smart matching
- ✅ **Application Tracking** - Know exactly where you stand in the hiring process
- ✅ **Skill Gap Analysis** - AI identifies areas for improvement and learning

### 🤖 AI Features (Powered by Google Gemini)
- ✅ **Resume Scoring** - Automatic candidate ranking from 0-100% match score
- ✅ **Smart Recommendations** - "Shortlist/Maybe/Reject" suggestions with reasoning
- ✅ **Skill Extraction** - Identify strengths and gaps from resumes automatically
- ✅ **Job Description Generation** - Create compelling, professional job posts in seconds
- ✅ **Candidate Comparison** - Rank multiple applicants against job requirements
- ✅ **Sentiment Analysis** - Analyze cover letters and candidate messages

## 🛠️ Tech Stack

### Backend
| Technology | Purpose |
|------------|---------|
| **.NET 9.0** | Core framework for all microservices |
| **MediatR** | CQRS pattern implementation |
| **FluentValidation** | Request validation |
| **Entity Framework Core** | ORM for database access |
| **JWT Authentication** | Secure token-based auth |
| **Ocelot** | API Gateway for routing |
| **Serilog** | Structured logging |
| **Swagger/OpenAPI** | API documentation |
| **SQL Server/PostgreSQL** | Database |

### Frontend
| Technology | Purpose |
|------------|---------|
| **React 19** | UI library |
| **TypeScript** | Type-safe JavaScript |
| **SCSS Modules** | Styling |
| **React Router 7** | Client-side routing |
| **Axios** | HTTP client |
| **Context API** | State management |
| **React Hooks** | Functional components |

### DevOps & Deployment
| Technology | Purpose |
|------------|---------|
| **Docker** | Containerization |
| **Render** | Backend hosting (5 services) |
| **Vercel** | Frontend hosting |
| **GitHub Actions** | CI/CD pipeline |
| **Git** | Version control |

## 📦 Microservices

### 1. AuthService (Port 7004)
Handles authentication, registration, and JWT token generation.

```csharp
// Endpoints
POST /api/Auth/register - Register new user (Candidate/Employer)
POST /api/Auth/login    - Login and receive JWT token
