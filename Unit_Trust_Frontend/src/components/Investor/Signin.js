import React, { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import '../../styles/common.css';
import Footer from '../Footer';
import Message from '../Message'; // 🔸 Import your reusable Message component

const SignIn = () => {
  const [formData, setFormData] = useState({
    email: "",
    password: "",
  });

  const [loginMessage, setLoginMessage] = useState("");
  const [messageType, setMessageType] = useState("success");

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.post("https://localhost:7182/api/user/login", formData);

      const userType = response.data.userType;
      const userName = response.data.name || "User";

      setLoginMessage(`🎉 Welcome back, ${userName}! Redirecting to your dashboard...`);
      setMessageType("success");

      if (response.data.token) {
        sessionStorage.setItem('authToken', response.data.token);
      }
      if (userType) {
        localStorage.setItem('role', userType);
      }

      setTimeout(() => {
        if (userType === "Investor") {
          navigate("/InvestorDashboard");
        } else if (userType === "Staff") {
          navigate("/StaffDashboard");
        } else {
          console.log("Unknown UserType");
        }
      }, 2000);

    } catch (error) {
      setLoginMessage(error.response?.data?.message || "Invalid username or password");
      setMessageType("error");
    }
  };

  return (
    <div>
      <form className="login-form" onSubmit={handleSubmit}>
        <h2>SIGN IN</h2>

        {loginMessage && <Message text={loginMessage} type={messageType} />} {/* 🔸 Use it here */}

        <div className="form-field">
          <input
            type="email"
            name="email"
            placeholder="Email"
            value={formData.email}
            onChange={handleChange}
            required
          />
        </div>
        <div className="form-field">
          <input
            type="password"
            name="password"
            placeholder="Password"
            value={formData.password}
            onChange={handleChange}
            required
          />
        </div>
        <button type="submit" className="submit-button">Login</button>

        <div className="form-links">
          <a href="/forgot-password">Forgot Password?</a>
          <a href="/signup">Don't have an account? Sign up</a>
        </div>
      </form>
      <Footer />
    </div>
  );
};

export default SignIn;
