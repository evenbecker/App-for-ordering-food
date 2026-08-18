import { Link } from 'react-router-dom';
import '../css/home.css';

export default function HomePage({isLoggedIn, forename, surname}) {
  return (
    <div className="container">
        <div className="header">
            <h1>Welcome to the Food Shop</h1>
            {isLoggedIn ? (
            <p>Nice to see you
                <span style={{color: 'blue'}}> {forename} {surname} </span>
                 again. You are able to take a look at {''}
                 <Link to="/products">Products Page </Link>
                 , 
                 <Link to="/account">Account </Link>
                 and {''}
                <Link to="/basket">Basket </Link>         
            </p>
             )
            : (
                <p>You are able to take a look at {''}
                  <Link to="/products">Products Page </Link>
            
                <br/> but you will have to {''}
                <Link to="/login">Login </Link>
                 or {''}
                 <Link to="/register">Register </Link>
                 {''}to see Account and Basket</p>
            )}
        </div>
		<div className="picture-links">
			<img src="./img/food1.jpeg" alt="The Picture of food"/>
	        <img src="./img/food2.jpeg" alt="The Picture of food"/>
		</div>
    </div>
  );
}