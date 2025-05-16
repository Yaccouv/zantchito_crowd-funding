import React, { useState } from "react";
import axios from "axios";
import Navbar from "../Navbar";
import Footer from "../Footer";
import "../../styles/common.css";
import Message from "../Message"; // 🔸 Import your reusable Message component

const EntrepreneurRegistration = () => {
  const [formData, setFormData] = useState({
    BusinessName: "",
    BusinessCategory: "",
    ProjectTitle: "",
    FundingGoal: "",
    BusinessDescription: "",
    BusinessImage: null, // For image upload
    IDImage: null, // For ID upload
    Status: "0", // Default status value
  });

  const [errors, setErrors] = useState({});
  const [message, setMessage] = useState(""); // State to hold success/error message
  const [messageType, setMessageType] = useState(""); // State to hold message type ('success' or 'error')

  // Handle form input changes
  const handleChange = (e) => {
    if (e.target.name === "BusinessImage" || e.target.name === "IDImage") {
      const file = e.target.files[0];
      if (file) {
        setFormData({ ...formData, [e.target.name]: file });
      }
    } else {
      setFormData({ ...formData, [e.target.name]: e.target.value });
    }
  };

  // Validate form data
  const validateForm = () => {
    const newErrors = {};
    if (!formData.BusinessName) newErrors.BusinessName = "Business name is required.";
    if (!formData.BusinessCategory) newErrors.BusinessCategory = "Business category is required.";
    if (!formData.ProjectTitle) newErrors.ProjectTitle = "Project title is required.";
    if (!formData.FundingGoal) newErrors.FundingGoal = "Funding goal is required.";
    if (!formData.BusinessDescription) newErrors.BusinessDescription = "Business description is required.";
    if (!formData.BusinessImage) newErrors.BusinessImage = "Business image is required.";
    if (!formData.IDImage) newErrors.IDImage = "ID image is required.";
    return newErrors;
  };

  // Handle form submission
  const handleSubmit = async (e) => {
    e.preventDefault();
    const validationErrors = validateForm();
    if (Object.keys(validationErrors).length > 0) {
      setErrors(validationErrors);
      return;
    }

    const formDataToSend = new FormData();
    for (const key in formData) {
      if (formData[key]) {
        formDataToSend.append(key, formData[key]); // Append files and other data
      }
    }

    try {
      const response = await axios.post(
        "https://localhost:7182/api/campaign/campaign",
        formDataToSend,
        {
          headers: { "Content-Type": "multipart/form-data" },
        }
      );
      setMessage(response.data.message); // Set success message
      setMessageType("success"); // Set message type to 'success'
      setFormData({
        BusinessName: "",
        BusinessCategory: "",
        ProjectTitle: "",
        FundingGoal: "",
        BusinessDescription: "",
        BusinessImage: null,
        IDImage: null,
        Status: "0",
      });
    } catch (error) {
      setMessage(error.response?.data?.message || "An error occurred."); // Set error message
      setMessageType("error"); // Set message type to 'error'
    }
  };

  return (
    <div className="dashboard-container">
      <Navbar />
      <div className="main-content">
        <div className="form-page-container">
          <div className="business">
            <form className="form-container" onSubmit={handleSubmit}>
              <h2>Campaign Registration</h2>

              {/* Display message if exists */}
              {message && <Message text={message} type={messageType} />}

              {/* Business Name */}
              <div className="form-field">
                <input
                  type="text"
                  placeholder="Business Name"
                  onChange={handleChange}
                  name="BusinessName"
                  value={formData.BusinessName}
                  required
                />
                {errors.BusinessName && <p className="error">{errors.BusinessName}</p>}
              </div>

              {/* Business Category */}
              <div className="form-field">
                <input
                  type="text"
                  placeholder="Business Category"
                  onChange={handleChange}
                  name="BusinessCategory"
                  value={formData.BusinessCategory}
                  required
                />
                {errors.BusinessCategory && <p className="error">{errors.BusinessCategory}</p>}
              </div>

              {/* Project Title */}
              <div className="form-field">
                <input
                  type="text"
                  placeholder="Project Title"
                  onChange={handleChange}
                  name="ProjectTitle"
                  value={formData.ProjectTitle}
                  required
                />
                {errors.ProjectTitle && <p className="error">{errors.ProjectTitle}</p>}
              </div>

              {/* Funding Goal */}
              <div className="form-field">
                <input
                  type="number"
                  placeholder="Funding Goal"
                  onChange={handleChange}
                  name="FundingGoal"
                  value={formData.FundingGoal}
                  required
                />
                {errors.FundingGoal && <p className="error">{errors.FundingGoal}</p>}
              </div>

              {/* Business Description */}
              <div className="form-field">
                <textarea
                  placeholder="Business Description"
                  onChange={handleChange}
                  name="BusinessDescription"
                  value={formData.BusinessDescription}
                  rows="5"
                  required
                />
                {errors.BusinessDescription && <p className="error">{errors.BusinessDescription}</p>}
              </div>

              {/* Image Upload */}
              <div className="form-field">
                <label htmlFor="imageUpload">Upload Business Sample Image</label>
                <input
                  type="file"
                  accept="image/*"
                  onChange={handleChange}
                  name="BusinessImage"
                  id="imageUpload"
                  required
                />
                {errors.BusinessImage && <p className="error">{errors.BusinessImage}</p>}
              </div>

              {/* ID Upload */}
              <div className="form-field">
                <label htmlFor="idUpload">Upload National ID (PDF)</label>
                <input
                  type="file"
                  accept="application/pdf"
                  onChange={handleChange}
                  name="IDImage"
                  id="idUpload"
                  required
                />
                {errors.IDImage && <p className="error">{errors.IDImage}</p>}
              </div>

              {/* Submit Button */}
              <button type="submit" className="submit-button">
                Submit
              </button>
            </form>
          </div>
        </div>
      </div>
      <Footer />
    </div>
  );
};

export default EntrepreneurRegistration;
