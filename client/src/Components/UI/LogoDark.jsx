import React from "react";

import "./Logo.css";
import logoDark from "../../images/Logo-dark.png";

const LogoDark = () => {
  return (
    <div className="logo-container">
      <img src={logoDark} className="logo-img" />
    </div>
  );
};

export default LogoDark;
