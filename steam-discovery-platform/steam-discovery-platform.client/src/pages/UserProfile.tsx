import { useEffect, useState } from 'react'
import { getMe } from '../services/userService';

function UserProfile() {
    interface UserDTO {
        username: string;
        email: string;
        createdAt: string;
        role: string;
    }

    const [userData, setUserData] = useState<UserDTO>();
    const [loading, setLoading] = useState(true);

    useEffect(() => {

        const fetchData = async () => {
            try {
                const data = await getMe();
                setUserData(data);
            } catch (err) {
                console.log(err);
            } finally {
                setLoading(false);
            }
        };
        fetchData();
    }, []);
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
                            {/* Avatar / Ikona */}
                            <div className="mb-4 d-inline-block p-3 rounded-circle " style={{ background: '#1b2838' }}>
                                <i className="bi bi-person-fill display-1 text-secondary"></i>
                            </div>

                            <h2 className="fw-bold mb-1" style={{ color: '#fff' }}>{userData?.username}</h2>
                            <span className="badge bg-danger text-light mb-4">{userData?.role}</span>

                            <div className="text-start mt-4">
                                <div className="mb-3">
                                    <label className="small text-light text-uppercase fw-bold">Email address</label>
                                    <p className="fs-5 mb-0 border-bottom border-secondary pb-2">{userData?.email}</p>
                                </div>

                                <div className="mb-3">
                                    <label className="small text-light text-uppercase fw-bold">Account created</label>
                                    <p className="fs-6 mb-0">
                                        {userData?.createdAt ? new Date(userData.createdAt).toLocaleDateString() : 'Brak danych'}
                                    </p>
                                </div>
                            </div>

                            <div className="d-grid gap-2 mt-5">
                                <button className="btn btn-outline-danger btn-">Edit profile</button>
                                <button className="btn btn-link text-light btn-sm text-decoration-none">Change password</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}
export default UserProfile;