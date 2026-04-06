import { useEffect, useState } from 'react'
import { getMe } from '../services/userService';

function UserProfile() {
    interface UserDTO {
        username: string;
        email: string;
        createdAt: Date;
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
return (
    <div className="text-light">
        <p>Debug: {JSON.stringify(userData)}</p> {/* To pokaże co faktycznie przyszło */}
        <p>User: {userData?.username}</p>
    </div>
);
}

export default UserProfile;