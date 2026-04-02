import { Link } from 'react-router-dom';

function RegisterPage() {
    return (
        <div className="container-fluid min-vh-100 d-flex justify-content-center bg-dark text-light"
            style={{
                background: 'linear-gradient(135deg, #1b2838 0%, #000000 100%)',
                fontFamily: '"Motiva Sans", Sans-serif'
            }}>

            <div className="row justify-content-center w-100 my-5">

                {/* back to home page */}
                <div className="container">
                    <Link to="/" className="text-decoration-none d-inline-flex align-items-center text-danger hover-opacity">
                        <i className="bi bi-chevron-left me-2"></i>
                        <span className="fw-bold uppercase tracking-wider">Back to Steam Discovery</span>
                    </Link>
                </div>

                <div className="col-11 col-sm-8 col-md-6 col-lg-5 col-xl-4">

                    {/* registration card */}
                    <div className="card  shadow-lg p-4"
                        style={{ borderRadius: '15px', backgroundColor: 'rgba(20, 25, 35, 0.95)' }}>

                        <div className="card-body">
                            {/* Header */}
                            <div className="text-center mb-4">
                                <div className="display-5 text-danger mb-2"
                                    style={{ filter: 'drop-shadow(0 0 10px rgba(220, 53, 69, 0.3))' }}>
                                    <i className="bi bi-person-plus-fill"></i>
                                </div>
                                <h3 className="fw-bold text-uppercase tracking-wider text-light mb-1">Create Account</h3>
                                <p className="small text-secondary opacity-75">Join the Steam Discovery community</p>
                            </div>

                            <form>
                                {/* Username */}
                                <div className="mb-3">
                                    <label className="form-label small text-secondary uppercase">Username</label>
                                    <div className="input-group">
                                        <span className="input-group-text bg-black border-secondary text-secondary">
                                            <i className="bi bi-person-badge"></i>
                                        </span>
                                        <input
                                            type="text"
                                            className="form-control bg-black text-light border-secondary shadow-none"
                                            placeholder="Choose a public name"
                                            maxLength={100}
                                            required
                                        />
                                    </div>
                                </div>

                                {/* Email */}
                                <div className="mb-3">
                                    <label className="form-label small text-secondary uppercase">Email Address</label>
                                    <div className="input-group">
                                        <span className="input-group-text bg-black border-secondary text-secondary">
                                            <i className="bi bi-envelope-at"></i>
                                        </span>
                                        <input
                                            type="email"
                                            className="form-control bg-black text-light border-secondary shadow-none"
                                            placeholder="example@domain.com"
                                            maxLength={255}
                                            required
                                        />
                                    </div>
                                </div>

                                {/* Password */}
                                <div className="mb-3">
                                    <label className="form-label small text-secondary uppercase">Password</label>
                                    <div className="input-group">
                                        <span className="input-group-text bg-black border-secondary text-secondary">
                                            <i className="bi bi-key-fill"></i>
                                        </span>
                                        <input
                                            type="password"
                                            className="form-control bg-black text-light border-secondary shadow-none"
                                            placeholder="Create a strong password"
                                            required
                                        />
                                    </div>
                                </div>

                                {/* Confirm Password */}
                                <div className="mb-4">
                                    <label className="form-label small text-secondary uppercase">Confirm Password</label>
                                    <div className="input-group">
                                        <span className="input-group-text bg-black border-secondary text-secondary">
                                            <i className="bi bi-shield-lock"></i>
                                        </span>
                                        <input
                                            type="password"
                                            className="form-control bg-black text-light border-secondary shadow-none"
                                            placeholder="Repeat password"
                                            required
                                        />
                                    </div>
                                </div>

                                {/* register button */}
                                <button className="btn btn-danger w-100 py-2 fw-bold mb-3 shadow-sm border-0"
                                    style={{ background: 'linear-gradient(to right, #e44d26, #f16529)' }}>
                                    Create My Account
                                </button>
                            </form>

                            <hr className="border-secondary opacity-25" />

                            {/* back to login */}
                            <div className="text-center mt-4">
                                <p className="small text-secondary mb-0">Already have an account?</p>
                                <Link to="/login" className="text-light fw-bold text-decoration-none hover-danger">
                                    Sign In here
                                </Link>
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

export default RegisterPage;