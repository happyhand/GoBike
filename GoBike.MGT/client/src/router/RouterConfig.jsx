import HomePage from "../containers/HomePage";
import ErrorPage from "../containers/ErrorPage";
import AccountManagerPage from "../containers/AccountManagerPage";
import MemberManagerPage from "../containers/MemberManagerPage";
import LoginPage from "../containers/LoginPage";

const routeMap = [
  {
    path: "/",
    component: HomePage,
    exact: true,
    requiresAuth: true
  },
  {
    path: "/Home",
    component: HomePage,
    routes: [
      {
        path: "/Home/Account",
        component: AccountManagerPage,
        requiresAuth: true
      },
      {
        path: "/Home/Member",
        component: MemberManagerPage,
        requiresAuth: true
      }
    ],
    requiresAuth: true
  },
  {
    path: "/Login",
    component: LoginPage
  },
  {
    path: "*",
    component: ErrorPage,
    routes: [
      {
        path: "*",
        component: ErrorPage
      }
    ]
  }
];

export default routeMap;
