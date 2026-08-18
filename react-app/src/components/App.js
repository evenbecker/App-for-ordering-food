import { useState, useEffect } from 'react';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import axios from 'axios';

import HomePage from '../pages/HomePage';
import AccountPage from '../pages/AccountPage';
import NotFoundPage from '../pages/NotFoundPage';
import Layout from './Layout';
import ProductsPage from '../pages/ProductsPage';
import LoginPage from '../pages/LoginPage';
import RegisterPage from '../pages/RegisterPage';
import BasketPage from '../pages/BasketPage';

export default function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [user, setUser] = useState({});

  const checkAuth = () => {
    const token = localStorage.getItem("token");
    if (!token) {
      setIsLoggedIn(false);
      return;
    }

    axios.get("http://localhost:5247/api/users/me", {
      headers: { Authorization: `Bearer ${token}` }
      })
      .then(res => {
        setUser(res.data);
        setIsLoggedIn(true);
      })
      .catch(() => {
        setIsLoggedIn(false);
        setUser({});
      });
  };

  useEffect(() => {
    checkAuth();
  }, []);

  //configure the router
  const router = createBrowserRouter([
    {
      path: "/",
      element: <Layout isLoggedIn={isLoggedIn} setIsLoggedIn={setIsLoggedIn} setUser={setUser}/>,
      children : [
        {
          index: true,
          element: <HomePage isLoggedIn={isLoggedIn} forename={user.forename} surname={user.surname} />
        },
        {
          path: "account",
          element: <AccountPage {...user}/>
        },
        {
          path: "products",
          element: <ProductsPage isLoggedIn={isLoggedIn}/>
        },
        {
          path: "login",
          element: <LoginPage onLoginSuccess={checkAuth}/>
        },
        {
          path: "register",
          element: <RegisterPage/>
        },
        {
          path: "basket",
          element: <BasketPage/>
        }
      ],
      errorElement: <NotFoundPage />
    }
  ]);

  return <RouterProvider router={router} />;
}

