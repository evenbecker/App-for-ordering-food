import React from "react";
import Header from "./Header";
import { Outlet } from "react-router-dom";

export default function Layout({ isLoggedIn, setIsLoggedIn, setUser }) {
  return (
    <div className="wrapper">
      <Header isLoggedIn={isLoggedIn} setIsLoggedIn={setIsLoggedIn} setUser={setUser} />
      <Outlet />
    </div>
  );
}