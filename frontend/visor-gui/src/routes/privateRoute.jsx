import React from "react";
import { Route } from "react-router-dom";
import { AuthConsumer } from "../providers/authProvider";

export const PrivateRoute = ({ component, ...rest }) => {
    const renderFn = (Component) => (props) => (
        <AuthConsumer>
            {({ isAuthenticated, signinRedirect }) => {
                if (!!Component && isAuthenticated()) {
                    debugger;
                    return <Component {...props} />;
                } else {
                    debugger;
                    signinRedirect();
                    return <span>loading</span>;
                }
            }}
        </AuthConsumer>
    );

    return <Route {...rest} render={renderFn(component)} />;
};