import React, {useContext, useState} from 'react';
import {Button, Container, Dropdown, Form} from "react-bootstrap";
import Header from "../components/Header";
import {getFreeTable, getFreeTableAsc, getFreeTableDesc} from "../http/tableApi";
import TablesContainer from "../components/TablesContainer";
import {AuthContext} from "../contexts";
import {createBooking} from "../http/Booking";

const Booking = () => {
    const {user} = useContext(AuthContext);
    const [tables, setTables] = useState([]);
    const [bookingDate, setBookingDate] = useState('');
    const [startTime, setStartTime] = useState('');
    const [endTime, setEndTime] = useState('');
    const [selectedSort, setSelectedSort] = useState('по умолчанию');

    const getTables = async () => {
        try {
            if(bookingDate === '' || startTime === '' || endTime === '') {
                alert("Введите все данные");
                return;
            }

            if(startTime > endTime) {
                alert("Пожалуйста, введите правильный временной диапазон: начало бронирования должно быть позже окончания.");
                return;
            }

            let freeTables = [];
            if(selectedSort === "по умолчанию") {
                freeTables = await getFreeTable(bookingDate, startTime, endTime);
            }else if(selectedSort === "по возрастанию") {
                freeTables = await getFreeTableAsc(bookingDate, startTime, endTime);
            }else {
                freeTables = await getFreeTableDesc(bookingDate, startTime, endTime);
            }

            setTables(freeTables);
        }catch (e) {
            console.log(e);
        }
    }

    const reserve = async (tableId) => {
        try {
            await createBooking(user.id, tableId, bookingDate, startTime, endTime);
        }catch (e) {
            console.log(e);
        }
    }

    const handleSortChange = (eventKey) => {
        setSelectedSort(eventKey);
    };

    return (
        <div>
            <Header/>
            <Container className="mb-4">
                <Form className="mb-4">
                    <Form.Group className="mb-3" controlId="bookingDate">
                        <Form.Label>Дата бронирования</Form.Label>
                        <Form.Control
                            type="date"
                            min={new Date().toISOString().split('T')[0]}
                            value={bookingDate}
                            onChange={(e) => setBookingDate(e.target.value)}
                        />
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="startTime">
                        <Form.Label>Время начала бронирования</Form.Label>
                        <Form.Control
                            type="time"
                            value={startTime}
                            onChange={(e) => setStartTime(e.target.value)}
                        />
                    </Form.Group>
                    <Form.Group className="mb-4" controlId="endTime">
                        <Form.Label>Время окончания бронирования</Form.Label>
                        <Form.Control
                            type="time"
                            value={endTime}
                            onChange={(e) => setEndTime(e.target.value)}
                        />
                    </Form.Group>

                    <Dropdown className="mb-4" onSelect={handleSortChange}>
                        <Dropdown.Toggle variant="secondary" id="sort-dropdown">
                            Сортировка: {selectedSort}
                        </Dropdown.Toggle>
                        <Dropdown.Menu>
                            <Dropdown.Item eventKey="по умолчанию">По умолчанию</Dropdown.Item>
                            <Dropdown.Item eventKey="по возрастанию">По возрастанию</Dropdown.Item>
                            <Dropdown.Item eventKey="по убыванию">По убыванию</Dropdown.Item>
                        </Dropdown.Menu>
                    </Dropdown>

                    <Button onClick={getTables}>Получить свободные столики</Button>
                </Form>

                <TablesContainer
                    tables={tables}
                    setTables={setTables}
                    reserveTable={reserve}
                />

            </Container>
        </div>
    );
};

export default Booking;