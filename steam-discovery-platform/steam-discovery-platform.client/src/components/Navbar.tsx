import React from 'react';
import { Link, useNavigate } from 'react-router-dom'
import { useAuth } from '../context/useAuth';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';

const Navbar: React.FC = () => {
    const { userName, logout } = useAuth();
    const navigate = useNavigate();

    const handleLogout = () => {
        logout();
        navigate('/');
    };

    return (
        <nav className="container-fluid px-4 py-3 d-flex justify-content-end align-items-center">
            {!userName ? (
                // not log in
                <div className="d-flex gap-2">
                    <button className="btn btn-outline-light rounded-pill btn-sm fw-bold border-0"
                        onClick={() => navigate('/login')}>
                        Sign In
                    </button>
                    <button className="btn btn-danger btn-sm fw-bold px-3 rounded-pill shadow-sm"
                        onClick={() => navigate('/registration')}>
                        Register
                    </button>
                </div>
            ) : (
                // log in
                <div className="dropdown">
                    <button
                        className="btn btn-outline-light dropdown-toggle rounded-pill px-3 shadow-none border-0 d-flex align-items-center gap-2"
                        type="button"
                        id="userMenu"
                        data-bs-toggle="dropdown"
                        aria-expanded="false"
                    >
                        <i className="bi bi-person-circle"></i>
                        <span>{userName}</span>
                    </button>

                    <ul className="dropdown-menu dropdown-menu-end dropdown-menu-dark shadow-lg border-secondary mt-2"
                        aria-labelledby="userMenu"
                        style={{ backgroundColor: '#1b2838', borderRadius: '10px' }}>
                        <li>
                            <Link className="dropdown-item py-2" to="/profile">
                                <i className="bi bi-person me-2"></i> My Profile
                            </Link>
                        </li>
                        <li>
                            <Link className="dropdown-item py-2" to="/favorites">
                                <i className="bi bi-heart me-2"></i> Favorites
                            </Link>
                        </li>
                        <li><hr className="dropdown-divider border-secondary opacity-25" /></li>
                        <li>
                            <button className="dropdown-item py-2 text-danger" onClick={handleLogout}>
                                <i className="bi bi-box-arrow-right me-2"></i> Logout
                            </button>
                        </li>
                    </ul>
                </div>
            )}
        </nav>
    );
};

export default Navbar;