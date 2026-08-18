import React, { useEffect, useState } from "react";
import axios from "axios";

export default function BasketPage() {
  const [orders, setOrders] = useState([]);
  const [error, setError] = useState(null);
  const token = localStorage.getItem("token");

  useEffect(() => {
    const fetchOrders = async () => {
      try {
        const res = await axios.get("http://localhost:5247/api/orders/allorders", {
          headers: {
            Authorization: `Bearer ${token}`
          }
        });
        setOrders(res.data);
      } catch (err) {
        setError("Failed to load order history");
      }
    };

    fetchOrders();
  }, [token]);

  if (error) return <div className="alert alert-danger">{error}</div>;

  return (
    <div className="container mt-4">
      <h2>Your Orders</h2>
      {orders.length === 0 ? (
        <p>You have no orders.</p>
      ) : (
        orders.map((order) => (
          <div className="border p-3 mb-5" key={order.orderno} id={`order-${order.orderno}`}>
            <h5>Order Number: {order.orderno}</h5>
            <table className="table mt-3">
              <thead className="table-dark">
                <tr>
                  <th>Image</th>
                  <th>Stock Number</th>
                  <th>Description</th>
                  <th>Price (£)</th>
                  <th>Quantity</th>
                  <th>Total (£)</th>
                </tr>
              </thead>
              <tbody>
                {order.items.map((item, i) => (
                  <tr key={`${order.orderno}-${item.stockno}-${i}`}>
                    <td>
                      <img
                        src={`/img/${item.stockno}.jpeg`}
                        alt={item.description}
                        style={{ width: "100px", objectFit: "cover" }}
                      />
                    </td>
                    <td>{item.stockno}</td>
                    <td>{item.description}</td>
                    <td>{item.price}</td>
                    <td>{item.quantity}</td>
                    <td>{(item.price * item.quantity).toFixed(2)}</td>
                  </tr>
                ))}
              </tbody>
            </table>
            <h5>Total Price: £{order.totalPrice.toFixed(2)}</h5>
          </div>
        ))
      )}
    </div>
  );
}
