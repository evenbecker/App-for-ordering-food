

export default function AccountPage({memberno, forename, surname, street, town, postcode, email, category}) {
  return (
    <div className="container">
        <div className="row">
            <div className="col">
                <table className="table">
                    <tbody>
                        <tr>
                            <th scope="row" className="table-dark">Customer Number</th>
                            <td>{memberno}</td>
                        </tr>
                        <tr>
                            <th scope="row" className="table-dark">Customer Forename</th>
                            <td>{forename}</td>
                        </tr>
                        <tr>
                            <th scope="row" className="table-dark">Customer Surname</th>
                            <td>{surname}</td>
                        </tr>
                        <tr>
                            <th scope="row" className="table-dark">Customer Street</th>
                            <td>{street}</td>
                        </tr>
                        <tr>
                            <th scope="row" className="table-dark">Customer Town</th>
                            <td>{town}</td>
                        </tr>
                        <tr>
                            <th scope="row" className="table-dark">Customer Postcode</th>
                            <td>{postcode}</td>
                        </tr>
                        <tr>
                            <th scope="row" className="table-dark">Customer Email</th>
                            <td>{email}</td>
                        </tr>
                        <tr>
                            <th scope="row" className="table-dark">Customer Category</th>
                            <td>{category}</td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div className="col">
                <div className="image ms-5 mt-5">
                    <img src={`./img/ecom${category}.png`} alt={`the picture of ${category} category`} style={{width: 150 + "px"}}/>
                </div>
            </div>
        </div>
    </div>
  );
}