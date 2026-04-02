import React from 'react';
import { Link } from 'react-router-dom';

function LoginPage() {
    return (
        <div className="container-fluid min-vh-100 bg-dark text-light"
            style={{
                background: 'linear-gradient(135deg, #1b2838 0%, #000000 100%)',
                fontFamily: '"Motiva Sans", Sans-serif'
            }}>

            {/* back to home page */}
            <div className="container py-3">
                <Link to="/" className="text-decoration-none d-inline-flex align-items-center text-danger hover-opacity">
                    <i className="bi bi-chevron-left me-2"></i>
                    <span className="fw-bold uppercase tracking-wider">Back to Steam Discovery</span>
                </Link>
            </div>

            <div className="row justify-content-center w-100">
                <div className="col-11 col-sm-8 col-md-6 col-lg-4 col-xl-3">

                    {/* login cart */}
                    <div className="card  border-dark shadow-lg p-4"
                        style={{ borderRadius: '15px', backgroundColor: 'rgba(20, 25, 35, 0.95)' }}>

                        <div className="card-body">
                            {/* Logo / icon */}
                            <div className="text-center mb-4">
                                <div className="display-4 text-danger mb-2"
                                    style={{ filter: 'drop-shadow(0 0 10px rgba(220, 53, 69, 0.3))' }}>
                                    <i className="bi bi-rocket-takeoff-fill"></i>
                                </div>
                                <h3 className="fw-bold text-uppercase tracking-wider text-light mb-1">
                                    Sign In
                                </h3>
                                <p className="small text-secondary opacity-75">
                                    Discovery Platform for Steam
                                </p>
                            </div>

                            <form>
                                {/* Input: Email / Username */}
                                <div className="mb-3">
                                    <label className="form-label small text-light uppercase">Account Name</label>
                                    <div className="input-group">
                                        <span className="input-group-text bg-black border-secondary text-secondary">
                                            <i className="bi bi-person-fill"></i>
                                        </span>
                                        <input
                                            type="text"
                                            className="form-control bg-black text-light border-secondary shadow-none"
                                            placeholder="Your username"
                                        />
                                    </div>
                                </div>

                                {/* Input: Password */}
                                <div className="mb-4">
                                    <div className="d-flex justify-content-between">
                                        <label className="form-label small text-light">Password</label>
                                        <a href="#" className="small text-danger text-decoration-none opacity-75">Forgot?</a>
                                    </div>
                                    <div className="input-group">
                                        <span className="input-group-text bg-black border-secondary text-secondary">
                                            <i className="bi bi-lock-fill"></i>
                                        </span>
                                        <input
                                            type="password"
                                            className="form-control bg-black text-light border-secondary shadow-none"
                                            placeholder=""
                                        />
                                    </div>
                                </div>

                                {/* login button */}
                                <button className="btn btn-danger w-100 py-2 fw-bold mb-3 shadow-sm border-0"
                                    style={{ background: 'linear-gradient(to right, #e44d26, #f16529)' }}>
                                    Sign In
                                </button>

                                {/* Checkbox: remember me */}
                                <div className="form-check mb-4">
                                    <input className="form-check-input bg-dark border-secondary shadow-none" type="checkbox" id="rememberMe" />
                                    <label className="form-check-label small text-secondary" htmlFor="rememberMe">
                                        Remember me
                                    </label>
                                </div>
                            </form>

                            <hr className="border-secondary opacity-25" />

                            {/* registration link */}
                            <div className="text-center mt-4">
                                <p className="small text-secondary mb-0">Don't have an account?</p>
                                <a href="#" className="text-light fw-bold text-decoration-none hover-danger">
                                    Join for Free
                                </a>
                            </div>
                        </div>
                    </div>

                    {/* footer */}
                    <div className="text-center mt-4 opacity-50">
                        <p className="x-small text-light" style={{ fontSize: '0.9rem' }}>
                            2026 Steam Discovery Platform. Not affiliated with Valve Corp.
                        </p>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default LoginPage;