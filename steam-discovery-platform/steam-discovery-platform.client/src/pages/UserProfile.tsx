import { useEffect, useState } from 'react';
import { getMe, updateUser } from '../services/userService';
interface UserDTO {
    username: string;
    email: string;
    createdAt: string;
    role: string;
}

function UserProfile() {
    const [userData, setUserData] = useState<UserDTO>();
    const [loading, setLoading] = useState(true);

    const [isEditing, setIsEditing] = useState(false);
    const [editName, setEditName] = useState('');
    const [editEmail, setEditEmail] = useState('');
    const [error, setError] = useState('');

    const fetchData = async () => {
        try {
            const data = await getMe();
            setUserData(data);
            setEditName(data.username);
            setEditEmail(data.email);
        } catch (err) {
            console.log(err);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchData();
    }, []);

    const handleUpdate = async () => {
        try {
            setError('');
            const updated = await updateUser({ userName: editName, email: editEmail });
            setUserData(updated);
            setIsEditing(false);
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        } catch (err: any) {
            setError(err.response?.data || "Update failed");
        }
    };

    if (loading) {
        return (
            <div className="d-flex justify-content-center align-items-center vh-100">
                <div className="spinner-border text-info" role="status">
                    <span className="visually-hidden">Loading...</span>
                </div>
            </div>
        );
    }

    return (
        <div className="container py-5">
            <div className="row justify-content-center">
                <div className="col-md-6 col-lg-5">
                    <div className="card border-0 shadow-lg" style={{
                        background: 'rgba(18, 24, 29, 0.95)',
                        color: '#dcdede',
                        borderRadius: '20px',
                        borderLeft: '4px solid #1a9fff'
                    }}>
                        <div className="card-body p-4 text-center">
                            <div className="mb-4 d-inline-block p-3 rounded-circle" style={{ background: '#1b2838' }}>
                                <i className="bi bi-person-fill display-1 text-secondary"></i>
                            </div>

                            {!isEditing ? (
                                <>
                                    <h2 className="fw-bold mb-1" style={{ color: '#fff' }}>{userData?.username}</h2>
                                </>
                            ) : (
                                <div className="mb-4">
                                    <h4 className="text-light">Editing Profile</h4>
                                </div>
                            )}

                            {error && <div className="alert alert-danger py-2 small">{error}</div>}

                            <div className="text-start mt-4">
                                {/* Username */}
                                <div className="mb-3">
                                    <label className="small text-danger text-uppercase fw-bold">Username</label>
                                    {isEditing ? (
                                        <input
                                            type="text"
                                            className="form-control bg-dark text-white border-secondary"
                                            value={editName}
                                            onChange={(e) => setEditName(e.target.value)}
                                        />
                                    ) : (
                                        <p className="fs-5 mb-0 border-bottom border-secondary pb-2">{userData?.username}</p>
                                    )}
                                </div>

                                {/* Email */}
                                <div className="mb-3">
                                    <label className="small text-danger text-uppercase fw-bold">Email address</label>
                                    {isEditing ? (
                                        <input
                                            type="email"
                                            className="form-control bg-dark text-white border-secondary"
                                            value={editEmail}
                                            onChange={(e) => setEditEmail(e.target.value)}
                                        />
                                    ) : (
                                        <p className="fs-5 mb-0 border-bottom border-secondary pb-2">{userData?.email}</p>
                                    )}
                                </div>

                                <div className="mb-3">
                                    <label className="small text-danger text-uppercase fw-bold">Account created</label>
                                    <p className="fs-6 mb-0 text-light">
                                        {userData?.createdAt ? new Date(userData.createdAt).toLocaleDateString() : 'Brak danych'}
                                    </p>
                                </div>
                            </div>

                            <div className="d-grid gap-2 mt-5">
                                {isEditing ? (
                                    <>
                                        <button className="btn btn-danger" onClick={handleUpdate}>Save Changes</button>
                                        <button className="btn btn-outline-light btn-sm" onClick={() => setIsEditing(false)}>Cancel</button>
                                    </>
                                ) : (
                                    <>
                                        <button className="btn btn-outline-danger" onClick={() => setIsEditing(true)}>Edit Profile</button>
                                        <button className="btn btn-link text-light btn-sm text-decoration-none">Change password</button>
                                    </>
                                )}
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default UserProfile;