import React, {useContext, useEffect, useState} from 'react';
import {AuthContext} from "../contexts";
import {deleteBooking, getBooking} from "../http/Booking";
import Header from "../components/Header";
import {Card, CloseButton, Container} from "react-bootstrap";

const MyBooking = () => {
    const {user} = useContext(AuthContext);
    const [bookings, setBooking] = useState([]);

    useEffect(() => {
        getBooking(user.id)
            .then(data => setBooking(data))
            .catch(e => console.log(e));
    }, []);

    const delBooking = async (id) => {
        try {
            await deleteBooking(id);
            setBooking(currentBooking => currentBooking.filter(b => b.id != id));
        }catch (e) {
            console.log(e);
        }
    }


    return (
        <div>
            <Header/>
            <Container>
                {bookings.length === 0 ? (
                    <p>У вас нет ни одного бронирования.</p>
                ) : (
                    <div className="d-flex flex-wrap">
                        {bookings.map(booking => (
                            <Card key={booking.id} className="m-2 pe-5 ps-2">
                                <Card.Body>
                                    <Card.Title className="mb-2">{booking["tableName"]}</Card.Title>
                                    <Card.Text className="mb-1">
                                        <strong>Дата:</strong> {new Date(booking.date).toLocaleDateString()}
                                    </Card.Text>
                                    <Card.Text className="mb-1">
                                        <strong>Начало:</strong> {booking["timeFrom"]}
                                    </Card.Text>
                                    <Card.Text className="mb-1">
                                        <strong>Конец:</strong> {booking["timeTo"]}
                                    </Card.Text>

                                    <CloseButton onClick={() => delBooking(booking.id)} className="position-absolute top-0 end-0 p-3"/>
                                </Card.Body>
                            </Card>
                        ))}
                    </div>
                )}
            </Container>
        </div>
    );
};

export default MyBooking;