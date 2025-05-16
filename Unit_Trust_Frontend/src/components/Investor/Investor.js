import React, { useState } from "react";
import axios from "axios";
import '../../styles/Investor/investor.css';  // Importing the CSS file

const Investor = () => {
    const [formData, setFormData] = useState({
        Firstname: "",
        Lastname: "",
        Gender: "",
        DateOfBirth: "",
        Email: "",
        PhoneNumber: "",
        City: "",
        EmploymentStatus: "",
        AnnualIncome: "",
        SourceOfFunds: "",
        BankAccountNumber: "",
        BankBranchCode: "",
        BankAccountType: "",
        InvestmentGoals: "",
        PaymentPlan: "",
        NextOfKinName: "",
        NextOfKinRelationship: "",
        NextOfKinPhone: "",
        IdNumber: "",
        Nationality: "", // Ensure Nationality field exists
    });

    const [errors, setErrors] = useState({});

    const handleChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    const validateForm = () => {
        const newErrors = {};
        if (!formData.Firstname) newErrors.Firstname = "First name is required.";
        if (!formData.Lastname) newErrors.Lastname = "Last name is required.";
        if (!formData.Gender) newErrors.Gender = "Gender is required.";
        if (!formData.DateOfBirth) newErrors.DateOfBirth = "Date of birth is required.";
        if (!formData.Email) newErrors.Email = "Email is required.";
        if (!formData.PhoneNumber) newErrors.PhoneNumber = "Phone number is required.";
        if (!formData.City) newErrors.City = "City is required.";
        if (!formData.EmploymentStatus) newErrors.EmploymentStatus = "Employment status is required.";
        if (!formData.AnnualIncome) newErrors.AnnualIncome = "Annual income is required.";
        if (!formData.SourceOfFunds) newErrors.SourceOfFunds = "Source of funds is required.";
        if (!formData.BankAccountNumber) newErrors.BankAccountNumber = "Bank account number is required.";
        if (!formData.BankBranchCode) newErrors.BankBranchCode = "Bank branch code is required.";
        if (!formData.BankAccountType) newErrors.BankAccountType = "Bank account type is required.";
        if (!formData.InvestmentGoals) newErrors.InvestmentGoals = "Investment goals are required.";
        if (!formData.PaymentPlan) newErrors.PaymentPlan = "Payment plan is required.";
        if (!formData.NextOfKinName) newErrors.NextOfKinName = "Next of kin name is required.";
        if (!formData.NextOfKinRelationship) newErrors.NextOfKinRelationship = "Next of kin relationship is required.";
        if (!formData.NextOfKinPhone) newErrors.NextOfKinPhone = "Next of kin phone number is required.";
        if (!formData.IdNumber) newErrors.IdNumber = "ID number is required.";  // Validate IdNumber
        if (!formData.Nationality) newErrors.Nationality = "Nationality is required."; // Validate Nationality
        return newErrors;
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        const validationErrors = validateForm();
        if (Object.keys(validationErrors).length > 0) {
            setErrors(validationErrors); // Set validation errors if any
            return;
        }

        console.log("Form Data:", formData); // Log the data being sent
        try {
            const response = await axios.post("https://localhost:7182/api/investor/investor", formData, {
                headers: { "Content-Type": "application/json" },
            });
            alert(response.data.message);
        } catch (error) {
            console.error("Error:", error.response?.data || error.message);
            alert(error.response?.data?.message || "An error occurred.");
        }
    };

    return (
        <form className="investor-form" onSubmit={handleSubmit}>
            <h2>Investment Form</h2>
            {['Firstname', 'Lastname', 'Email', 'PhoneNumber', 'City', 'EmploymentStatus', 'AnnualIncome', 'SourceOfFunds', 'BankAccountNumber', 'BankBranchCode', 'BankAccountType', 'InvestmentGoals', 'PaymentPlan', 'NextOfKinName', 'NextOfKinRelationship', 'NextOfKinPhone', 'IdNumber', 'Nationality'].map((field, idx) => (
                <div key={idx} className="form-field">
                    <input
                        type={field === "Email" ? "email" : "text"}
                        placeholder={field.replace(/([A-Z])/g, ' $1').trim()}
                        onChange={handleChange}
                        name={field}
                        value={formData[field]}
                        required
                    />
                    {errors[field] && <p className="error">{errors[field]}</p>}
                </div>
            ))}
            <div className="form-field">
                <select
                    onChange={handleChange}
                    name="Gender"
                    value={formData.Gender}
                    required
                >
                    <option value="">Select Gender</option>
                    <option value="Male">Male</option>
                    <option value="Female">Female</option>
                    <option value="Other">Other</option>
                </select>
                {errors.Gender && <p className="error">{errors.Gender}</p>}
            </div>
            <div className="form-field">
                <input
                    type="date"
                    placeholder="Date of Birth"
                    onChange={handleChange}
                    name="DateOfBirth"
                    value={formData.DateOfBirth}
                    required
                />
                {errors.DateOfBirth && <p className="error">{errors.DateOfBirth}</p>}
            </div>
            <button type="submit" className="submit-button">Submit</button>
        </form>
    );
};

export default Investor;
