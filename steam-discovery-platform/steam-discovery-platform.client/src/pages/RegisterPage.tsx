/* eslint-disable @typescript-eslint/no-explicit-any */
import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import api from '../services/api';

function RegisterPage() {

    interface RegisterErrors {
        username?: string;
        email?: string;
        password?: string;
        confirmPassword?: string;
        [key: string]: string | undefined;
    }

    interface RegisterData {
        username: string;
        email: string;
        password: string;
        confirmPassword: string;
    }

    const [formData, setFormData] = useState<RegisterData>({
        username: '',
        email: '',
        password: '',
        confirmPassword: ''
    });

    const [errors, setErrors] = useState<RegisterErrors>({});

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setFormData(prev => ({ ...prev, [name]: value }));

        if (errors[name] || errors.server) {
            setErrors(prev => ({ ...prev, [name]: '', server: '' }));
        }
    };

    const validate = (): boolean => {
        const newErrors: RegisterErrors = {};

        if (formData.username.length < 3) {
            newErrors.username = "Username must be at least 3 characters.";
        }

        if (!formData.email.includes('@')) {
            newErrors.email = "Please enter a valid email.";
        }

        if (formData.password.length < 6) {
            newErrors.password = "Password must be at least 6 characters.";
        }

        if (formData.password !== formData.confirmPassword) {
            newErrors.confirmPassword = "Passwords do not match!";
        }

        setErrors(newErrors);

        return Object.keys(newErrors).length === 0;
    };

    const navigate = useNavigate();
    const [loading, setLoading] = useState(false);

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        if (!validate()) return;

        try {
            setLoading(true);
            setErrors({});

            const registerPayload = { ...formData };
            delete (registerPayload as any).confirmPassword;

            await api.post("/api/users/register", registerPayload);
            navigate("/login");
        } catch (err: any) {
            const data = err.response?.data;

            let message = "Registration failed.";

            if (typeof data === 'string') {
                message = data;
            } else if (data?.message) {
                message = data.message;
            } else if (data?.errors) {
                // Jeœli backend zwraca b³êdy w formacie ModelState (obiekt z tablicami)
                message = Object.values(data.errors).flat().join(" ");
            }

            setErrors(prev => ({ ...prev, server: message }));
            console.error("Backend error formatted:", message);
        } finally {
            setLoading(false);
        }
    };

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

                            <form onSubmit={handleSubmit}>
                                {/* Username */}
                                <div className="mb-3">
                                    <label className="form-label small text-light uppercase">Username</label>
                                    <div className="input-group">
                                        <span className="input-group-text bg-black border-dark text-secondary">
                                            <i className="bi bi-person-badge"></i>
                                        </span>
                                        <input
                                            name="username"
                                            type="text"
                                            className="form-control bg-black text-light border-dark shadow-none"
                                            placeholder="Choose a public name"
                                            maxLength={100}
                                            value={formData.username}
                                            onChange={handleChange}
                                            required
                                        />
                                    </div>
                                    {errors.username && <div className="text-danger small mt-1">{errors.username}</div>}
                                </div>

                                {/* Email */}
                                <div className="mb-3">
                                    <label className="form-label small text-light uppercase">Email Address</label>
                                    <div className="input-group">
                                        <span className="input-group-text bg-black border-dark text-secondary">
                                            <i className="bi bi-envelope-at"></i>
                                        </span>
                                        <input
                                            name="email"
                                            type="email"
                                            className="form-control bg-black text-light border-dark shadow-none"
                                            placeholder="example@domain.com"
                                            maxLength={255}
                                            value={formData.email}
                                            onChange={handleChange}
                                            required
                                        />
                                    </div>
                                    {errors.email && <div className="text-danger small mt-1">{errors.email}</div>}
                                </div>

                                {/* Password */}
                                <div className="mb-3">
                                    <label className="form-label small text-light uppercase">Password</label>
                                    <div className="input-group">
                                        <span className="input-group-text bg-black border-dark text-secondary">
                                            <i className="bi bi-key-fill"></i>
                                        </span>
                                        <input
                                            name="password"
                                            type="password"
                                            className="form-control bg-black text-light border-dark shadow-none"
                                            placeholder="Create a strong password"
                                            value={formData.password}
                                            onChange={handleChange}
                                            required
                                        />
                                    </div>
                                    {errors.password && <div className="text-danger small mt-1">{errors.password}</div>}
                                </div>

                                {/* Confirm Password */}
                                <div className="mb-4">
                                    <label className="form-label small text-light uppercase">Confirm Password</label>
                                    <div className="input-group">
                                        <span className="input-group-text bg-black border-dark text-secondary">
                                            <i className="bi bi-shield-lock"></i>
                                        </span>
                                        <input
                                            name="confirmPassword"
                                            type="password"
                                            className="form-control bg-black text-light border-dark shadow-none"
                                            placeholder="Repeat password"
                                            value={formData.confirmPassword}
                                            onChange={handleChange}
                                            required
                                        />
                                    </div>
                                    {errors.confirmPassword && <div className="text-danger small mt-1">{errors.confirmPassword}</div>}
                                </div>

                                {errors.server && (
                                    <div className="alert alert-danger d-flex align-items-center border-0 mb-3"
                                        style={{ backgroundColor: 'rgba(220, 53, 69, 0.2)', color: '#ff6b6b' }}>
                                        <i className="bi bi-exclamation-triangle-fill me-2"></i>
                                        <small>{errors.server}</small>
                                    </div>
                                )}

                                {/* register button */}
                                <button
                                    disabled={loading}
                                    className="btn btn-danger w-100 py-2 fw-bold mb-3 shadow-sm border-0"
                                    style={{ background: 'linear-gradient(to right, #e44d26, #f16529)' }}>
                                    {loading ? (
                                        <span className="spinner-border spinner-border-sm me-2"></span>
                                    ) : "Create My Account"}
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