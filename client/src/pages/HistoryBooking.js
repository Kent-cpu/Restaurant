import React, {useEffect, useState} from 'react';
import Header from "../components/Header";
import {Container, Table} from "react-bootstrap";
import {getAllBooking} from "../http/Booking";

const HistoryBooking = () => {
    const [bookings, setBookings] = useState([]);

    
    useEffect(() => {
        getAllBooking()
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
                        <th>Имя пользователя</th>
                        <th>Номер столика</th>
                        <th>Дата бронирования</th>
                    </tr>
                    </thead>
                    <tbody>
                    {bookings.map(booking => (
                        <tr key={booking.id}>
                            <td>{booking.username}</td>
                            <td>{booking["tableName"]}</td>
                            <td>{new Date(booking.date).toLocaleDateString()}</td>
                        </tr>
                    ))}
                    </tbody>
                </Table>
            </Container>
        </div>
    );
};

export default HistoryBooking;