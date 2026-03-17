// src/pages/Apply/Apply.tsx
import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { createApplicationApi } from "../../api/applicationApi";
import { getJobByIdApi } from "../../api/jobApi";
import { useAuth } from "../../context/AuthContext";
import { Job } from "../../types";
import styles from "./Apply.module.scss";

const Apply: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { user } = useAuth();
  const [job, setJob] = useState<Job | null>(null);
  const [loading, setLoading] = useState(false);
  const [success, setSuccess] = useState(false);
  const [error, setError] = useState("");

  const [form, setForm] = useState({
    candidateName: user?.fullName || "",
    candidateEmail: user?.email || "",
    resumeText: "",
    coverLetter: "",
  });

  useEffect(() => {
    fetchJob();
  }, [id]);

  const fetchJob = async () => {
    try {
      const response = await getJobByIdApi(Number(id));
      if (response.success) setJob(response.data);
    } catch (err) {
      console.error(err);
    }
  };

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError("");

    try {
      const response = await createApplicationApi({
        jobId: Number(id),
        candidateName: form.candidateName,
        candidateEmail: form.candidateEmail,
        resumeText: form.resumeText,
        coverLetter: form.coverLetter,
      });

      if (response.success) {
        setSuccess(true);
      } else {
        setError(response.errors[0] || "Application failed");
      }
    } catch (err: any) {
      setError(err.response?.data?.errors?.[0] || "Something went wrong!");
    } finally {
      setLoading(false);
    }
  };

  if (success) {
    return (
      <div className={styles.successContainer}>
        <div className={styles.successCard}>
          <div className={styles.successIcon}>🎉</div>
          <h2>Application Submitted!</h2>
          <p>
            Your application for <strong>{job?.title}</strong> has been
            submitted successfully!
          </p>
          <button onClick={() => navigate("/jobs")}>Browse More Jobs</button>
        </div>
      </div>
    );
  }

  return (
    <div className={styles.container}>
      <div className={styles.card}>
        <h2>Apply for {job?.title}</h2>
        <p className={styles.company}>
          🏢 {job?.company} • 📍 {job?.location}
        </p>

        {error && <div className={styles.error}>{error}</div>}

        <form onSubmit={handleSubmit}>
          <div className={styles.field}>
            <label>Full Name</label>
            <input
              type="text"
              name="candidateName"
              value={form.candidateName}
              onChange={handleChange}
              required
            />
          </div>

          <div className={styles.field}>
            <label>Email</label>
            <input
              type="email"
              name="candidateEmail"
              value={form.candidateEmail}
              onChange={handleChange}
              required
            />
          </div>

          <div className={styles.field}>
            <label>Resume Text</label>
            <textarea
              name="resumeText"
              placeholder="Paste your resume content here..."
              value={form.resumeText}
              onChange={handleChange}
              rows={8}
              required
            />
          </div>

          <div className={styles.field}>
            <label>Cover Letter (Optional)</label>
            <textarea
              name="coverLetter"
              placeholder="Write a brief cover letter..."
              value={form.coverLetter}
              onChange={handleChange}
              rows={4}
            />
          </div>

          <div className={styles.actions}>
            <button
              type="button"
              className={styles.backBtn}
              onClick={() => navigate(`/jobs/${id}`)}
            >
              ← Back
            </button>
            <button
              type="submit"
              className={styles.submitBtn}
              disabled={loading}
            >
              {loading ? "Submitting..." : "Submit Application 🚀"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default Apply;
