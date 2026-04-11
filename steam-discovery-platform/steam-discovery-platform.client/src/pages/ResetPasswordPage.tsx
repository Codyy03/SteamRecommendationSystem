import { useState } from 'react';
import api from '../services/api';

function ChangePassword() {
    interface ChangePasswordDto {
        password: string;   
        newPassword: string;
    }
    const [password, setCurrentPassword] = useState('');
    const [newPassword, setNewPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');

    const [error, setError] = useState<string | null>(null);
    const [success, setSuccess] = useState<string | null>(null);
    const [loading, setLoading] = useState(false);
    const [showPasswords, setShowPasswords] = useState(false);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError(null);
        setSuccess(null);

        // 1. Walidacja po stronie frontendu
        if (!password || !newPassword || !confirmPassword) {
            setError("All fields are required.");
            return;
        }

        if (newPassword !== confirmPassword) {
            setError("New passwords do not match!");
            return;
        }

        if (newPassword.length < 6) {
            setError("New password must be at least 6 characters long.");
            return;
        }

        // 2. Wysłanie zapytania do API
        try {
            setLoading(true);

            const data: ChangePasswordDto = {
                password: password,
                newPassword: newPassword
            };
            await api.put('/api/users/passwordReset', data);

            setSuccess("Your password has been changed successfully!");
            setCurrentPassword('');
            setNewPassword('');
            setConfirmPassword('');

            // eslint-disable-next-line @typescript-eslint/no-explicit-any
        } catch (err: any) {
            setError(err.response?.data || "Failed to change password. Please try again.");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="container-fluid py-5 min-vh-100 bg-dark d-flex align-items-center justify-content-center">
            <div className="container" style={{ maxWidth: '500px' }}>

                <div className="card border-secondary shadow-lg" style={{ backgroundColor: '#111923' }}>
                    <div className="card-header border-secondary bg-transparent pt-4 pb-3">
                        <h3 className="text-white text-center fw-bold mb-0">
                            <i className="bi bi-shield-lock me-2 text-info"></i>
                            Change Password
                        </h3>
                    </div>

                    <div className="card-body p-4">
                        {/* notifications */}
                        {error && (
                            <div className="alert alert-danger d-flex align-items-center py-2" role="alert">
                                <i className="bi bi-exclamation-triangle-fill me-2"></i>
                                <div>{error}</div>
                            </div>
                        )}
                        {success && (
                            <div className="alert alert-success d-flex align-items-center py-2" role="alert">
                                <i className="bi bi-check-circle-fill me-2"></i>
                                <div>{success}</div>
                            </div>
                        )}

                        <form onSubmit={handleSubmit}>
                            {/* old password */}
                            <div className="mb-3">
                                <label className="form-label text-secondary small mb-1">Current Password</label>
                                <div className="input-group">
                                    <span className="input-group-text bg-black border-secondary text-secondary">
                                        <i className="bi bi-key"></i>
                                    </span>
                                    <input
                                        type={showPasswords ? "text" : "password"}
                                        className="form-control bg-black border-secondary text-white shadow-none"
                                        placeholder="Enter current password"
                                        value={password}
                                        onChange={(e) => setCurrentPassword(e.target.value)}
                                    />
                                </div>
                            </div>

                            {/* new password */}
                            <div className="mb-3">
                                <label className="form-label text-secondary small mb-1">New Password</label>
                                <div className="input-group">
                                    <span className="input-group-text bg-black border-secondary text-secondary">
                                        <i className="bi bi-lock"></i>
                                    </span>
                                    <input
                                        type={showPasswords ? "text" : "password"}
                                        className="form-control bg-black border-secondary text-white shadow-none"
                                        placeholder="Enter new password"
                                        value={newPassword}
                                        onChange={(e) => setNewPassword(e.target.value)}
                                    />
                                </div>
                            </div>

                            {/* repeat password */}
                            <div className="mb-4">
                                <label className="form-label text-secondary small mb-1">Confirm New Password</label>
                                <div className="input-group">
                                    <span className="input-group-text bg-black border-secondary text-secondary">
                                        <i className="bi bi-lock-fill"></i>
                                    </span>
                                    <input
                                        type={showPasswords ? "text" : "password"}
                                        className="form-control bg-black border-secondary text-white shadow-none"
                                        placeholder="Confirm new password"
                                        value={confirmPassword}
                                        onChange={(e) => setConfirmPassword(e.target.value)}
                                    />
                                </div>
                            </div>

                            {/* show password*/}
                            <div className="mb-4 form-check">
                                <input
                                    type="checkbox"
                                    className="form-check-input bg-black border-secondary"
                                    id="showPasswordCheck"
                                    checked={showPasswords}
                                    onChange={() => setShowPasswords(!showPasswords)}
                                    style={{ cursor: 'pointer' }}
                                />
                                <label className="form-check-label text-secondary small" htmlFor="showPasswordCheck" style={{ cursor: 'pointer' }}>
                                    Show passwords
                                </label>
                            </div>

                            {/* Submit */}
                            <button
                                type="submit"
                                className="btn btn-info w-100 fw-bold py-2 shadow-sm"
                                disabled={loading}
                            >
                                {loading ? (
                                    <>
                                        <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
                                        Updating...
                                    </>
                                ) : (
                                    <>
                                        <i className="bi bi-check2-circle me-2"></i>
                                        Save New Password
                                    </>
                                )}
                            </button>
                        </form>
                    </div>
                </div>

            </div>
        </div>
    );
}

export default ChangePassword;