import { Link } from 'react-router-dom';

export default function Header({ isLoggedIn, setIsLoggedIn, setUser }) {

    const handleLogout = () => {
        localStorage.removeItem("token");
        setIsLoggedIn(false);
        setUser({});
    };

    return (
        <nav className="navbar bg-primary mb-3" data-bs-theme="dark">
            <div className="container-fluid">
                <span className="navbar-brand mb-0 h1">Food Shop</span>
                <div className="nav nav-underline me-auto">
                    <Link className="nav-link" to="/">Home</Link>
                    <Link className="nav-link" to="products">Products</Link>
                    <Link className={`nav-link ${!isLoggedIn ? ' disabled' : ''}`} to="account">Account</Link>
                    <Link className={`nav-link ${!isLoggedIn ? ' disabled' : ''}`} to="basket">Basket</Link>
                    <Link className={`nav-link ${isLoggedIn ? ' disabled' : ''}`} to="login">Login</Link>
                    <Link className={`nav-link ${isLoggedIn ? ' disabled' : ''}`} to="register">Register</Link>
                    <Link className={`nav-link ${!isLoggedIn ? ' disabled' : ''}`} to="/"
                    onClick={handleLogout}>Logout</Link>
                </div>
            </div>
        </nav>
    )
}