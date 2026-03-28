import React from 'react';

const Navbar: React.FC = () => {
    return (
        <nav className="container-fluid px-4 py-3 d-flex justify-content-end align-items-center">
            <div className="d-flex gap-2">
                <button className="btn btn-outline-light rounded-pill btn-sm fw-bold border-0">
                    Sign In
                </button>
                <button className="btn btn-danger btn-sm fw-bold px-3 rounded-pill shadow-sm">
                    Register
                </button>
            </div>
        </nav>
    );

};

export default Navbar;