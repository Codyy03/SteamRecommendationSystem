import React from 'react';
import { useNavigate } from 'react-router-dom'

const Navbar: React.FC = () => {

    const navigate = useNavigate();

    const navigateToLogin = (destination: string) => {
        navigate(destination)
    }

    return (
        <nav className="container-fluid px-4 py-3 d-flex justify-content-end align-items-center">
            <div className="d-flex gap-2">
                <button className="btn btn-outline-light rounded-pill btn-sm fw-bold border-0"
                    onClick={() => navigateToLogin('/login')}>
                    Sign In
                </button>
                <button className="btn btn-danger btn-sm fw-bold px-3 rounded-pill shadow-sm"
                    onClick={() => navigateToLogin('/registration')}>
                    Register
                </button>
            </div>
        </nav>
    );

};

export default Navbar;