import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";

import "./Forms.css";
import LogoLight from "../UI/LogoLight";

const LoginForm = () => {
  const [formData, setFormData] = useState({
    email: "",
    password: "",
  });
  const navigate = useNavigate();

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.post(
        "https://localhost:7191/auth/Auth/Login",
        formData,
        { withCredentials: true }
      );
      if (response.data.success) {
        console.log(response.data.message);
        navigate("/");
      } else {
        console.log("Error!");
      }
    } catch (ex) {
      console.error(ex.message);
    }
  };

  return (
    <div className="form-container">
      <LogoLight />
      <h2 className="form-title">Login</h2>
      <Form onSubmit={handleSubmit}>
        <Form.Group className="mb-3" controlId="loginEmail">
          <Form.Label className="form-label">Email Address</Form.Label>
          <Form.Control
            type="email"
            name="email"
            value={formData.email}
            onChange={handleChange}
            placeholder="Enter Email"
          />
          <Form.Text className="text-muted">
            Enter the email address of your account.
          </Form.Text>
        </Form.Group>
        <Form.Group className="mb-3" controlId="loginPassword">
          <Form.Label>Password</Form.Label>
          <Form.Control
            type="password"
            name="password"
            value={formData.password}
            onChange={handleChange}
            placeholder="Password"
          />
        </Form.Group>
        <div className="text-center">
          <Button type="submit" variant="primary" className="w-50 fs-5">
            Login
          </Button>
        </div>
      </Form>
    </div>
  );
};

export default LoginForm;
