//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using PathCreation;
//using com.limphus.utilities;
//
//namespace com.limphus.convoy
//{
//    public class PathFollower : MonoBehaviour
//    {
//        [SerializeField] private PathCreator pathCreator;
//        [SerializeField] private EndOfPathInstruction endOfPathInstruction;
//        [SerializeField] private float speed = 5;
//
//        private float distanceTravelled;
//
//        public void SetPath(PathCreator newPath)
//        {
//            if (pathCreator != null)
//            {
//                //unsubscribe from the old pathUpdated event
//                pathCreator.pathUpdated -= OnPathChanged;
//            }
//
//            pathCreator = newPath;
//
//            if (pathCreator != null)
//            {
//                //and subscribe to the pathUpdated event so that we're notified if the path changes during the game
//                pathCreator.pathUpdated += OnPathChanged;
//            }
//        }
//
//        private void Start()
//        {
//            if (pathCreator != null)
//            {
//                //subscribed to the pathUpdated event so that we're notified if the path changes during the game
//                pathCreator.pathUpdated += OnPathChanged;
//            }
//        }
//
//        private void OnDestroy()
//        {
//            if (pathCreator != null)
//            {
//                //and unsubscribe from the pathUpdated event
//                pathCreator.pathUpdated -= OnPathChanged;
//            }
//        }
//
//        private void Update()
//        {
//            if (PauseManager.IsPaused) return;
//
//            if (pathCreator != null)
//            {
//                distanceTravelled += speed * Time.deltaTime;
//                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
//                transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
//            }
//        }
//
//        //if the path changes during the game, update the distance travelled so that the follower's position on the new path
//        //is as close as possible to its position on the old path
//        private void OnPathChanged()
//        {
//            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
//        }
//    }
//}