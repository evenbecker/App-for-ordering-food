import { Link } from 'react-router-dom';

export default function NotFoundPage() {
  return (
    <div className="container">
      <img src="./img/fail.png" alt="unable to load image" style={{width: 300 +"px", marginTop: 100  + "px", marginBottom: 20 + "px"}}/>
      <h1>404 Not Found</h1>
      <p>The page you are looking for does not exist.</p>
      <Link to="/">Go back to Home </Link>
    </div>
  );
}