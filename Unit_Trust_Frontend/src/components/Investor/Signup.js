import React, { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import Message from "../Message"; // Import Message component
import '../../styles/common.css';
import Footer from '../Footer';  // Correct import path

const SignUp = () => {
  const [formData, setFormData] = useState({
    firstName: "",
    lastName: "",
    email: "",
    password: "",
    confirmPassword: "",
  });

  const [errors, setErrors] = useState({}); // For storing validation errors
  const [pageMessage, setPageMessage] = useState(""); // For the page message

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const validateForm = () => {
    const newErrors = {};
    if (!formData.firstName) {
      newErrors.firstName = "First name is required.";
    }
    if (!formData.lastName) {
      newErrors.lastName = "Last name is required.";
    }
    if (!formData.email) {
      newErrors.email = "Email is required.";
    }
    if (!formData.password) {
      newErrors.password = "Password is required.";
    }
    if (formData.password !== formData.confirmPassword) {
      newErrors.confirmPassword = "Passwords do not match.";
    }
    return newErrors;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const validationErrors = validateForm();
    if (Object.keys(validationErrors).length > 0) {
      setErrors(validationErrors);
      return;
    }

    try {
      const response = await axios.post("https://localhost:7182/api/user/signup", formData);
      setPageMessage(`🎉 Success! ${response.data.message}`); // Set success message
    } catch (error) {
      setPageMessage(`❌ Error: ${error.response?.data?.message || "An error occurred"}`); // Set error message
    }
  };

  return (
    <div>
      {/* Display the message conditionally */}
      {pageMessage && <Message text={pageMessage} type={pageMessage.startsWith("🎉") ? "success" : "error"} />}

      <form className="login-form" onSubmit={handleSubmit}>
        <h2>SIGN UP</h2>

        {/* First Name */}
        <div className="form-field">
          <input
            type="text"
            name="firstName"
            placeholder="First Name"
            value={formData.firstName}
            onChange={handleChange}
            required
          />
          {errors.firstName && <p className="error">{errors.firstName}</p>}
        </div>

        {/* Last Name */}
        <div className="form-field">
          <input
            type="text"
            name="lastName"
            placeholder="Last Name"
            value={formData.lastName}
            onChange={handleChange}
            required
          />
          {errors.lastName && <p className="error">{errors.lastName}</p>}
        </div>

        {/* Email */}
        <div className="form-field">
          <input
            type="email"
            name="email"
            placeholder="Email"
            value={formData.email}
            onChange={handleChange}
            required
          />
          {errors.email && <p className="error">{errors.email}</p>}
        </div>

        {/* Password */}
        <div className="form-field">
          <input
            type="password"
            name="password"
            placeholder="Password"
            value={formData.password}
            onChange={handleChange}
            required
          />
          {errors.password && <p className="error">{errors.password}</p>}
        </div>

        {/* Confirm Password */}
        <div className="form-field">
          <input
            type="password"
            name="confirmPassword"
            placeholder="Confirm Password"
            value={formData.confirmPassword}
            onChange={handleChange}
            required
          />
          {errors.confirmPassword && <p className="error">{errors.confirmPassword}</p>}
        </div>

        <button type="submit" className="submit-button">Register</button>

        {/* Sign In Link */}
        <div className="form-links">
          <a href="/signin">Already have an account? Sign in</a>
        </div>
      </form>

      {/* Footer component rendered below the form */}
      <Footer />
    </div>
  );
};

export default SignUp;
