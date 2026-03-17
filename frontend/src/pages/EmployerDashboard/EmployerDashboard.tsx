// src/pages/EmployerDashboard/EmployerDashboard.tsx
import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { getApplicationsByJobIdApi } from "../../api/applicationApi";
import { createJobApi, deleteJobApi, getAllJobsApi } from "../../api/jobApi";
import { Application, CreateJobDto, Job } from "../../types";
import styles from "./EmployerDashboard.module.scss";

const EmployerDashboard: React.FC = () => {
  const navigate = useNavigate();
  const [jobs, setJobs] = useState<Job[]>([]);
  const [applications, setApplications] = useState<Application[]>([]);
  const [selectedJobId, setSelectedJobId] = useState<number | null>(null);
  const [activeTab, setActiveTab] = useState<"jobs" | "post">("jobs");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  const [form, setForm] = useState<CreateJobDto>({
    title: "",
    company: "",
    description: "",
    location: "",
    salary: 0,
    jobType: "FullTime",
  });

  useEffect(() => {
    fetchJobs();
  }, []);

  const fetchJobs = async () => {
    try {
      const response = await getAllJobsApi();
      if (response.success) setJobs(response.data);
    } catch (err) {
      setError("Failed to load jobs.");
    } finally {
      setLoading(false);
    }
  };

  const fetchApplications = async (jobId: number) => {
    try {
      const response = await getApplicationsByJobIdApi(jobId);
      if (response.success) {
        setApplications(response.data);
        setSelectedJobId(jobId);
      }
    } catch (err) {
      setError("Failed to load applications.");
    }
  };

  const handleDeleteJob = async (jobId: number) => {
    if (!window.confirm("Are you sure you want to delete this job?")) return;
    try {
      await deleteJobApi(jobId);
      setJobs(jobs.filter((j) => j.jobId !== jobId));
      setSuccess("Job deleted successfully!");
      setTimeout(() => setSuccess(""), 3000);
    } catch (err) {
      setError("Failed to delete job.");
    }
  };

  const handlePostJob = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError("");
    try {
      const response = await createJobApi(form);
      if (response.success) {
        setSuccess("Job posted successfully!");
        setActiveTab("jobs");
        fetchJobs();
        setForm({
          title: "",
          company: "",
          description: "",
          location: "",
          salary: 0,
          jobType: "FullTime",
        });
        setTimeout(() => setSuccess(""), 3000);
      }
    } catch (err) {
      setError("Failed to post job.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className={styles.container}>
      <div className={styles.header}>
        <h1>👔 Employer Dashboard</h1>
        <p>Manage your job postings and applications</p>
      </div>

      {error && <div className={styles.error}>{error}</div>}
      {success && <div className={styles.success}>{success}</div>}

      <div className={styles.tabs}>
        <button
          className={activeTab === "jobs" ? styles.active : ""}
          onClick={() => setActiveTab("jobs")}
        >
          📋 My Jobs ({jobs.length})
        </button>
        <button
          className={activeTab === "post" ? styles.active : ""}
          onClick={() => setActiveTab("post")}
        >
          ➕ Post New Job
        </button>
        <button onClick={() => navigate("/ai-dashboard")}>🤖 AI Tools</button>
      </div>

      {activeTab === "jobs" && (
        <div className={styles.jobsList}>
          {loading && <p>Loading...</p>}
          {jobs.map((job) => (
            <div key={job.jobId} className={styles.jobItem}>
              <div className={styles.jobInfo}>
                <h3>{job.title}</h3>
                <span>
                  {job.company} • {job.location} • {job.jobType}
                </span>
              </div>
              <div className={styles.jobActions}>
                <button
                  className={styles.viewBtn}
                  onClick={() => fetchApplications(job.jobId)}
                >
                  👥 Applications
                </button>
                <button
                  className={styles.deleteBtn}
                  onClick={() => handleDeleteJob(job.jobId)}
                >
                  🗑️ Delete
                </button>
              </div>
            </div>
          ))}

          {selectedJobId && (
            <div className={styles.applications}>
              <h3>
                Applications for Job #{selectedJobId}({applications.length})
              </h3>
              {applications.length === 0 ? (
                <p>No applications yet.</p>
              ) : (
                applications.map((app) => (
                  <div key={app.applicationId} className={styles.appItem}>
                    <div>
                      <strong>{app.candidateName}</strong>
                      <span>{app.candidateEmail}</span>
                    </div>
                    <span
                      className={styles.status}
                      style={{
                        background:
                          app.status === "Shortlisted"
                            ? "#d4edda"
                            : app.status === "Rejected"
                            ? "#f8d7da"
                            : "#fff3cd",
                        color:
                          app.status === "Shortlisted"
                            ? "#155724"
                            : app.status === "Rejected"
                            ? "#721c24"
                            : "#856404",
                      }}
                    >
                      {app.status}
                    </span>
                  </div>
                ))
              )}
            </div>
          )}
        </div>
      )}

      {activeTab === "post" && (
        <div className={styles.postJob}>
          <form onSubmit={handlePostJob}>
            <div className={styles.field}>
              <label>Job Title</label>
              <input
                type="text"
                placeholder="e.g. Senior .NET Developer"
                value={form.title}
                onChange={(e) => setForm({ ...form, title: e.target.value })}
                required
              />
            </div>

            <div className={styles.field}>
              <label>Company</label>
              <input
                type="text"
                placeholder="e.g. TechCorp"
                value={form.company}
                onChange={(e) => setForm({ ...form, company: e.target.value })}
                required
              />
            </div>

            <div className={styles.row}>
              <div className={styles.field}>
                <label>Location</label>
                <input
                  type="text"
                  placeholder="e.g. Pune, MH"
                  value={form.location}
                  onChange={(e) =>
                    setForm({ ...form, location: e.target.value })
                  }
                  required
                />
              </div>

              <div className={styles.field}>
                <label>Salary (₹)</label>
                <input
                  type="number"
                  placeholder="e.g. 800000"
                  value={form.salary}
                  onChange={(e) =>
                    setForm({ ...form, salary: Number(e.target.value) })
                  }
                />
              </div>
            </div>

            <div className={styles.field}>
              <label>Job Type</label>
              <select
                value={form.jobType}
                onChange={(e) => setForm({ ...form, jobType: e.target.value })}
              >
                <option value="FullTime">Full Time</option>
                <option value="PartTime">Part Time</option>
                <option value="Remote">Remote</option>
              </select>
            </div>

            <div className={styles.field}>
              <label>Job Description</label>
              <textarea
                placeholder="Describe the role, responsibilities and requirements..."
                value={form.description}
                onChange={(e) =>
                  setForm({ ...form, description: e.target.value })
                }
                rows={6}
                required
              />
            </div>

            <button
              type="submit"
              className={styles.submitBtn}
              disabled={loading}
            >
              {loading ? "Posting..." : "🚀 Post Job"}
            </button>
          </form>
        </div>
      )}
    </div>
  );
};

export default EmployerDashboard;
