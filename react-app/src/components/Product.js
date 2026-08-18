export default function Product({ stockno, description, price, qtyinstock })  {
    return (
        <div className="item">
            <img src={`./img/${stockno}.jpeg`} alt={`the picture of ${stockno} product`} />
            <div>{stockno}</div>
            <div className="description">{description}</div>
            <div>{price} £</div>
            <div>
                <select form="orderform" name={stockno}>
                    {Array.from({ length: qtyinstock + 1}, (_, i) => (
                        <option key={`${stockno}-${i}`} value={i}>{i}</option>
                    ))}
                </select>
            </div>
        </div>
    );
}