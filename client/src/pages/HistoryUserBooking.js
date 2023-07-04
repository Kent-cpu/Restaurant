import React, {useContext, useEffect, useState} from 'react';
import Header from "../components/Header";
import {getUserHistoryBooking} from "../http/Booking";
import {Container, Table} from "react-bootstrap";
import {AuthContext} from "../contexts";

const HistoryUserBooking = () => {
    const [bookings, setBookings] = useState([]);
    const {user} = useContext(AuthContext);

    useEffect(() => {
        getUserHistoryBooking(user.id)
            .then(data => setBookings(data))
            .catch(e => console.log(e));
    }, []);


    return (
        <div>
            <Header/>
            <Container>
                <Table bordered>
                    <thead>
                    <tr>
                        <th>Номер столика</th>
                        <th>Дата бронирования</th>
                        <th>Начало бронирования</th>
                        <th>Конец бронирования</th>
                    </tr>
                    </thead>
                    <tbody>
                    {bookings.map(booking => (
                        <tr key={booking.id}>
                            <td>{booking["tableName"]}</td>
                            <td>{new Date(booking.date).toLocaleDateString()}</td>
                            <td>{booking["timeFrom"]}</td>
                            <td>{booking["timeTo"]}</td>
                        </tr>
                    ))}
                    </tbody>
                </Table>
            </Container>
        </div>
    );
};

export default HistoryUserBooking;