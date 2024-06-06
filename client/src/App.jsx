import { Route, Routes } from "react-router-dom";
import "./App.css";

import LoginPage from "./Pages/LoginPage";

function App() {
  return (
    <Routes>
      <Route path="/" element={<h1>Root</h1>} />
      <Route path="/login" element={<LoginPage />} />
      <Route path="/register" element={<h1>Register</h1>} />
    </Routes>
  );
}

export default App;
